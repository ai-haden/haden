using System;
using System.Collections.Generic;
using Haden.NxtSDK;
using Haden.RobotBehavior;

namespace Haden.HardwareSmoke
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            bool seekMaxLightMode = HasFlag(args, "--seek-max-light");
            bool scorecardMode = HasFlag(args, "--scorecard");
            string port = ResolvePort(args);
            int retries = ReadIntEnv("HADEN_AUTOCONNECT_RETRIES", 5);
            int delayMs = ReadIntEnv("HADEN_AUTOCONNECT_DELAY_MS", 1000);

            Console.WriteLine("Haden hardware smoke starting...");
            Console.WriteLine("Port: " + port);
            Console.WriteLine("Retries: " + retries + ", DelayMs: " + delayMs);
            Console.WriteLine("Mode: " + (seekMaxLightMode ? "seek-max-light" : "smoke"));

            try
            {
                if (scorecardMode)
                {
                    PrintScorecard();
                    return 0;
                }

                using var client = new NxtBrickClient(port);
                client.ConnectWithRetry(retries, delayMs);
                client.KeepAlive();
                if (seekMaxLightMode)
                {
                    RunSeekMaxLight(client);
                }
                else
                {
                    int battery = client.GetBatteryLevel();
                    Console.WriteLine("Connected to NXT. Battery mV: " + battery);
                }

                client.Disconnect();
                Console.WriteLine("Disconnect clean.");
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Hardware smoke failed: " + ex.Message);
                return 1;
            }
        }

        private static string ResolvePort(string[] args)
        {
            if (args != null)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    string candidate = args[i];
                    if (!string.IsNullOrWhiteSpace(candidate) && !candidate.StartsWith("--", StringComparison.Ordinal))
                    {
                        return candidate.Trim();
                    }
                }
            }

            string env = Environment.GetEnvironmentVariable("HADEN_NXT_PORT");
            if (!string.IsNullOrWhiteSpace(env))
            {
                return env.Trim();
            }

            return "/dev/rfcomm0";
        }

        private static int ReadIntEnv(string name, int defaultValue)
        {
            string raw = Environment.GetEnvironmentVariable(name);
            if (!string.IsNullOrWhiteSpace(raw) && int.TryParse(raw, out int parsed) && parsed >= 0)
            {
                return parsed;
            }

            return defaultValue;
        }

        private static bool ReadBoolEnv(string name, bool defaultValue = false)
        {
            string raw = Environment.GetEnvironmentVariable(name);
            if (string.IsNullOrWhiteSpace(raw))
            {
                return defaultValue;
            }

            raw = raw.Trim();
            if (raw == "1")
            {
                return true;
            }

            if (raw == "0")
            {
                return false;
            }

            if (bool.TryParse(raw, out bool parsed))
            {
                return parsed;
            }

            return defaultValue;
        }

        private static bool HasFlag(string[] args, string flag)
        {
            if (args == null)
            {
                return false;
            }

            for (int i = 0; i < args.Length; i++)
            {
                if (string.Equals(args[i], flag, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static void RunSeekMaxLight(NxtBrickClient client)
        {
            int battery = client.GetBatteryLevel();
            Console.WriteLine("Connected to NXT. Battery mV: " + battery);

            NxtSensorPort lightSensorPort = ReadSensorPortEnv("HADEN_LIGHT_SENSOR_PORT", NxtSensorPort.Port3);
            NxtSensorPort bumpSensorPort = ReadSensorPortEnv("HADEN_BUMP_SENSOR_PORT", NxtSensorPort.Port1);
            NxtMotorPort scanMotorPort = ReadMotorPortEnv("HADEN_LIGHT_SCAN_MOTOR_PORT", NxtMotorPort.PortA);
            NxtMotorPort leftWheelPort = ReadMotorPortEnv("HADEN_LEFT_WHEEL_MOTOR_PORT", NxtMotorPort.PortB);
            NxtMotorPort rightWheelPort = ReadMotorPortEnv("HADEN_RIGHT_WHEEL_MOTOR_PORT", NxtMotorPort.PortC);

            int maxIterations = ReadIntEnv("HADEN_SEEK_MAX_ITERATIONS", ReadIntEnv("HADEN_SEEK_ITERATIONS", 300));
            int settleDelayMs = ReadIntEnv("HADEN_SEEK_SETTLE_DELAY_MS", 600);
            bool activeLight = ReadIntEnv("HADEN_LIGHT_SENSOR_ACTIVE", 0) == 1;
            bool bumpActiveLow = ReadBoolEnv("HADEN_BUMP_ACTIVE_LOW", false);
            int wheelStepDegrees = ReadIntEnv("HADEN_WHEEL_STEP_DEGREES", 35);
            bool steerInvert = ReadBoolEnv("HADEN_STEER_INVERT", false);
            bool scanInvert = ReadBoolEnv("HADEN_SCAN_INVERT", false);
            bool centerScanOnStart = ReadBoolEnv("HADEN_CENTER_SCAN_ON_START", true);
            int centerSweepDegrees = ReadIntEnv("HADEN_CENTER_SWEEP_DEGREES", 160);
            int centerPower = ReadIntEnv("HADEN_CENTER_POWER", 22);
            int smoothWindow = Math.Clamp(ReadIntEnv("HADEN_LIGHT_SMOOTH_WINDOW", 3), 1, 10);
            string databasePath = ReadStringEnv("HADEN_RL_DB_PATH", "output/haden-rl.db");
            var smoother = new LightSignalSmoother(smoothWindow);
            using var store = new SqliteExperimentStore(databasePath);
            ScorecardSummary summary = store.GetScorecardSummary();

            var policy = new PeakLightSteeringPolicy(
                scanMotorPower: ReadIntEnv("HADEN_SCAN_POWER", 18),
                scanDegreesMin: ReadIntEnv("HADEN_SCAN_DEGREES_MIN", 10),
                scanDegreesMax: ReadIntEnv("HADEN_SCAN_DEGREES_MAX", 35),
                scanDegreesStep: ReadIntEnv("HADEN_SCAN_DEGREES_STEP", 5),
                wheelBasePower: ReadIntEnv("HADEN_WHEEL_BASE_POWER", 35),
                wheelMaxPower: ReadIntEnv("HADEN_WHEEL_MAX_POWER", 70),
                wheelTurnGain: ReadIntEnv("HADEN_WHEEL_TURN_GAIN", 2),
                wheelTurnFloor: ReadIntEnv("HADEN_WHEEL_TURN_FLOOR", 6),
                deltaDeadband: ReadIntEnv("HADEN_SEEK_DELTA_DEADBAND", 2),
                peakTolerance: ReadIntEnv("HADEN_PEAK_TOLERANCE", 2));

            Console.WriteLine(
                "Seek setup: lightSensor=" + lightSensorPort +
                ", bumpSensor=" + bumpSensorPort +
                ", scanMotor=" + scanMotorPort +
                ", leftWheel=" + leftWheelPort +
                ", rightWheel=" + rightWheelPort +
                ", maxIterations=" + maxIterations +
                ", wheelStepDegrees=" + wheelStepDegrees +
                ", active=" + activeLight +
                ", bumpActiveLow=" + bumpActiveLow +
                ", centerScanOnStart=" + centerScanOnStart +
                ", smoothWindow=" + smoothWindow +
                ", steerInvert=" + steerInvert +
                ", scanInvert=" + scanInvert +
                ", rlDbPath=" + databasePath);
            Console.WriteLine(
                "Scorecard loaded: entries=" + summary.Entries +
                ", avgConfidence=" + summary.AverageConfidence.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture));

            if (centerScanOnStart)
            {
                CenterScanMotorAtStart(client, scanMotorPort, centerSweepDegrees, centerPower, settleDelayMs);
            }

            int completedIterations = 0;
            string stopReason = "max-iterations";
            var startedAt = DateTime.UtcNow;
            long sessionId = store.StartSession(
                startedAt,
                (int)lightSensorPort + 1,
                (int)bumpSensorPort + 1,
                maxIterations);
            double totalReward = 0.0;

            while (true)
            {
                if (maxIterations > 0 && completedIterations >= maxIterations)
                {
                    stopReason = "max-iterations";
                    break;
                }

                int rawSensor = client.ReadLightSensorValue(lightSensorPort, activeLight);
                int smoothedSensor = smoother.AddSample(rawSensor);
                PeakLightSteeringStep step = policy.Advance(smoothedSensor);
                bool bumpPressed = ReadBumpPressed(client, bumpSensorPort, bumpActiveLow);
                double reward = LightSeekRewardSignal.Compute(step.Delta, bumpPressed);
                totalReward += reward;

                int scanMotorPower = scanInvert ? -step.ScanMotorPower : step.ScanMotorPower;
                int leftWheelPower = step.LeftWheelPower;
                int rightWheelPower = step.RightWheelPower;

                if (steerInvert)
                {
                    int swap = leftWheelPower;
                    leftWheelPower = rightWheelPower;
                    rightWheelPower = swap;
                }

                if (!bumpPressed)
                {
                    client.TurnMotor(scanMotorPort, scanMotorPower, Math.Abs(step.ScanDegrees));
                    client.TurnMotor(leftWheelPort, leftWheelPower, Math.Abs(wheelStepDegrees));
                    client.TurnMotor(rightWheelPort, rightWheelPower, Math.Abs(wheelStepDegrees));
                }
                else
                {
                    stopReason = "bump-pressed";
                }

                DateTime nowUtc = DateTime.UtcNow;
                store.AppendStep(
                    sessionId,
                    completedIterations,
                    nowUtc,
                    rawSensor,
                    smoothedSensor,
                    step.Delta,
                    reward,
                    bumpPressed,
                    step.ScanDirection,
                    scanMotorPower,
                    leftWheelPower,
                    rightWheelPower);

                string stateKey = BuildStateKey(smoothedSensor, step.Delta, bumpPressed);
                string actionKey = BuildActionKey(step.ScanDirection, leftWheelPower, rightWheelPower);
                ScorecardSnapshot scorecard = store.AppendRlPoint(
                    sessionId,
                    stateKey,
                    actionKey,
                    reward,
                    bumpPressed,
                    nowUtc);

                Console.WriteLine(
                    "iter=" + completedIterations +
                    " sensorRaw=" + rawSensor +
                    " sensorSmooth=" + smoothedSensor +
                    " delta=" + step.Delta +
                    " reward=" + reward.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) +
                    " confidence=" + scorecard.Confidence.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture) +
                    " peak=" + step.PeakLightValue +
                    " stableTicks=" + step.PeakStableTicks +
                    " recoveries=" + step.RecoveryEvents +
                    " bump=" + bumpPressed +
                    " scanDir=" + step.ScanDirection +
                    " scanPwr=" + scanMotorPower +
                    " leftPwr=" + leftWheelPower +
                    " rightPwr=" + rightWheelPower);

                client.KeepAlive();
                if (settleDelayMs > 0)
                {
                    System.Threading.Thread.Sleep(settleDelayMs);
                }

                completedIterations++;

                if (bumpPressed)
                {
                    break;
                }
            }

            client.BrakeMotor(scanMotorPort);
            client.BrakeMotor(leftWheelPort);
            client.BrakeMotor(rightWheelPort);
            DateTime endedAt = DateTime.UtcNow;
            double elapsedSeconds = (endedAt - startedAt).TotalSeconds;
            bool success = string.Equals(stopReason, "bump-pressed", StringComparison.Ordinal);
            store.CompleteSession(
                sessionId,
                endedAt,
                stopReason,
                completedIterations,
                policy.PeakLightValue,
                totalReward,
                success);
            Console.WriteLine(
                "Seek complete. reason=" + stopReason +
                ", iterations=" + completedIterations +
                ", totalReward=" + totalReward.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) +
                ", elapsedSec=" + elapsedSeconds.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture));
        }

        private static bool ReadBumpPressed(NxtBrickClient client, NxtSensorPort bumpSensorPort, bool activeLow)
        {
            bool pressed = client.ReadTouchSensorPressed(bumpSensorPort);
            return activeLow ? !pressed : pressed;
        }

        private static string BuildStateKey(int sensorSmooth, int delta, bool bumpPressed)
        {
            string lightBucket = sensorSmooth >= 55 ? "high" : (sensorSmooth >= 35 ? "mid" : "low");
            string deltaBucket = delta > 1 ? "rise" : (delta < -1 ? "fall" : "flat");
            string bump = bumpPressed ? "1" : "0";
            return "light:" + lightBucket + ";delta:" + deltaBucket + ";bump:" + bump;
        }

        private static string BuildActionKey(int scanDirection, int leftPower, int rightPower)
        {
            int wheelDelta = rightPower - leftPower;
            int abs = Math.Abs(wheelDelta);
            string steer = wheelDelta > 0 ? "right" : (wheelDelta < 0 ? "left" : "straight");
            string magnitude = abs == 0 ? "0" : (abs <= 15 ? "1" : (abs <= 35 ? "2" : "3"));
            string scan = scanDirection > 0 ? "+" : (scanDirection < 0 ? "-" : "0");
            return "scan:" + scan + ";steer:" + steer + ";mag:" + magnitude;
        }

        private static void CenterScanMotorAtStart(
            NxtBrickClient client,
            NxtMotorPort scanMotorPort,
            int sweepDegrees,
            int power,
            int settleDelayMs)
        {
            int safeSweep = Math.Clamp(sweepDegrees, 20, 360);
            int safePower = Math.Clamp(power, 5, 70);
            int halfSweep = Math.Max(10, safeSweep / 2);

            Console.WriteLine(
                "Centering scan motor: sweep=" + safeSweep +
                ", power=" + safePower +
                ", halfSweep=" + halfSweep);

            client.TurnMotor(scanMotorPort, -safePower, safeSweep);
            if (settleDelayMs > 0)
            {
                System.Threading.Thread.Sleep(Math.Max(150, settleDelayMs / 2));
            }

            client.TurnMotor(scanMotorPort, safePower, halfSweep);
            if (settleDelayMs > 0)
            {
                System.Threading.Thread.Sleep(Math.Max(150, settleDelayMs / 2));
            }

            client.BrakeMotor(scanMotorPort);
        }

        private static void PrintScorecard()
        {
            string databasePath = ReadStringEnv("HADEN_RL_DB_PATH", "output/haden-rl.db");
            using var store = new SqliteExperimentStore(databasePath);
            ScorecardSummary summary = store.GetScorecardSummary();
            Console.WriteLine("Scorecard DB: " + databasePath);
            Console.WriteLine(
                "Summary: entries=" + summary.Entries +
                ", avgConfidence=" + summary.AverageConfidence.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture));

            IReadOnlyList<ScorecardEntry> top = store.GetTopScorecardEntries(limit: 5);
            Console.WriteLine("Top confidence entries:");
            if (top.Count == 0)
            {
                Console.WriteLine("  (none)");
            }
            else
            {
                for (int i = 0; i < top.Count; i++)
                {
                    ScorecardEntry row = top[i];
                    Console.WriteLine(
                        "  " + (i + 1) +
                        " conf=" + row.Confidence.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture) +
                        " q=" + row.QValue.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture) +
                        " updates=" + row.Updates +
                        " success=" + row.Successes +
                        " state=[" + row.StateKey + "]" +
                        " action=[" + row.ActionKey + "]");
                }
            }

            IReadOnlyList<ScorecardEntry> low = store.GetLowConfidenceEntries(limit: 5);
            Console.WriteLine("Lowest confidence entries:");
            if (low.Count == 0)
            {
                Console.WriteLine("  (none)");
            }
            else
            {
                for (int i = 0; i < low.Count; i++)
                {
                    ScorecardEntry row = low[i];
                    Console.WriteLine(
                        "  " + (i + 1) +
                        " conf=" + row.Confidence.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture) +
                        " q=" + row.QValue.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture) +
                        " updates=" + row.Updates +
                        " success=" + row.Successes +
                        " state=[" + row.StateKey + "]" +
                        " action=[" + row.ActionKey + "]");
                }
            }
        }

        private static NxtSensorPort ReadSensorPortEnv(string name, NxtSensorPort defaultPort)
        {
            int value = ReadIntEnv(name, (int)defaultPort + 1) - 1;
            if (value >= (int)NxtSensorPort.Port1 && value <= (int)NxtSensorPort.Port4)
            {
                return (NxtSensorPort)value;
            }

            return defaultPort;
        }

        private static NxtMotorPort ReadMotorPortEnv(string name, NxtMotorPort defaultPort)
        {
            int value = ReadIntEnv(name, (int)defaultPort);
            if (value >= (int)NxtMotorPort.PortA && value <= (int)NxtMotorPort.PortC)
            {
                return (NxtMotorPort)value;
            }

            return defaultPort;
        }

        private static string ReadStringEnv(string name, string defaultValue)
        {
            string raw = Environment.GetEnvironmentVariable(name);
            return string.IsNullOrWhiteSpace(raw) ? defaultValue : raw.Trim();
        }
    }
}
