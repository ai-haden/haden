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
            bool homeScanHeadMode = HasFlag(args, "--home-scan-head") || HasFlag(args, "--center-only");
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
                if (homeScanHeadMode)
                {
                    RunHomeScanHead(client);
                }
                else if (seekMaxLightMode)
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
            int scanProbePower = ReadIntEnv("HADEN_SCAN_PROBE_POWER", 22);
            int scanProbeDegrees = ReadIntEnv("HADEN_SCAN_PROBE_DEGREES", 20);
            int scanProbeSettleMs = ReadIntEnv("HADEN_SCAN_PROBE_SETTLE_MS", 120);
            int scanProbePowerMax = ReadIntEnv("HADEN_SCAN_PROBE_POWER_MAX", 40);
            int scanProbeDegreesMax = ReadIntEnv("HADEN_SCAN_PROBE_DEGREES_MAX", 60);
            int scanProbePowerStep = ReadIntEnv("HADEN_SCAN_PROBE_POWER_STEP", 4);
            int scanProbeDegreesStep = ReadIntEnv("HADEN_SCAN_PROBE_DEGREES_STEP", 8);
            int scanProbeFlatLimit = ReadIntEnv("HADEN_SCAN_PROBE_FLAT_LIMIT", 3);
            int minDecisionConfidencePercent = ReadIntEnv("HADEN_DECISION_MIN_CONFIDENCE_PERCENT", 6);
            bool scanHomeEnable = ReadBoolEnvAlias("HADEN_SCAN_HOME_ENABLE", "HADEN_CENTER_SCAN_ON_START", true);
            bool scanHomeDisable = ReadBoolEnvAlias("HADEN_SCAN_HOME_DISABLE", "HADEN_CENTER_DISABLE", false);
            bool scanHomeInvert = ReadBoolEnvAlias("HADEN_SCAN_HOME_INVERT", "HADEN_CENTER_INVERT", false);
            int scanHomePower = ReadIntEnvAlias("HADEN_SCAN_HOME_POWER", "HADEN_CENTER_POWER", 22);
            int scanHomeSettleMs = ReadIntEnvAlias("HADEN_SCAN_HOME_SETTLE_MS", "HADEN_CENTER_SETTLE_MS", 250);
            int scanHomeMaxSweepDegrees = ReadIntEnvAlias("HADEN_SCAN_HOME_MAX_SWEEP_DEGREES", "HADEN_CENTER_MAX_SWEEP_DEGREES", 1080);
            int scanHomeSweepStepDegrees = ReadIntEnvAlias("HADEN_SCAN_HOME_SWEEP_STEP_DEGREES", "HADEN_CENTER_SWEEP_STEP_DEGREES", 40);
            int scanHomeStagnantSteps = ReadIntEnvAlias("HADEN_SCAN_HOME_STAGNANT_STEPS", "HADEN_CENTER_STAGNANT_STEPS", 3);
            int wheelBasePower = ReadIntEnv("HADEN_WHEEL_BASE_POWER", 35);
            int wheelMaxPower = ReadIntEnv("HADEN_WHEEL_MAX_POWER", 70);
            int wheelTurnGain = ReadIntEnv("HADEN_WHEEL_TURN_GAIN", 2);
            int wheelTurnFloor = ReadIntEnv("HADEN_WHEEL_TURN_FLOOR", 6);
            int decisionDeadband = ReadIntEnvAlias("HADEN_DECISION_DEADBAND", "HADEN_SEEK_DELTA_DEADBAND", 2);
            int peakTolerance = ReadIntEnv("HADEN_PEAK_TOLERANCE", 2);
            int smoothWindow = Math.Clamp(ReadIntEnv("HADEN_LIGHT_SMOOTH_WINDOW", 3), 1, 10);
            string databasePath = ReadStringEnv("HADEN_RL_DB_PATH", "output/haden-rl.db");
            var smoother = new LightSignalSmoother(smoothWindow);
            using var store = new SqliteExperimentStore(databasePath);
            ScorecardSummary summary = store.GetScorecardSummary();

            var decisionPolicy = new LightTripletDecisionPolicy(
                basePower: wheelBasePower,
                maxPower: wheelMaxPower,
                turnGain: wheelTurnGain,
                turnFloor: wheelTurnFloor,
                deadband: decisionDeadband);

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
                ", scanHomeEnable=" + scanHomeEnable +
                ", scanHomeDisable=" + scanHomeDisable +
                ", scanHomeInvert=" + scanHomeInvert +
                ", scanProbePower=" + scanProbePower +
                ", scanProbePowerMax=" + scanProbePowerMax +
                ", scanProbePowerStep=" + scanProbePowerStep +
                ", scanProbeDegrees=" + scanProbeDegrees +
                ", scanProbeDegreesMax=" + scanProbeDegreesMax +
                ", scanProbeDegreesStep=" + scanProbeDegreesStep +
                ", scanProbeSettleMs=" + scanProbeSettleMs +
                ", scanProbeFlatLimit=" + scanProbeFlatLimit +
                ", minDecisionConfidencePercent=" + minDecisionConfidencePercent +
                ", smoothWindow=" + smoothWindow +
                ", steerInvert=" + steerInvert +
                ", scanInvert=" + scanInvert +
                ", rlDbPath=" + databasePath);
            Console.WriteLine(
                "Scorecard loaded: entries=" + summary.Entries +
                ", avgConfidence=" + summary.AverageConfidence.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture));

            if (scanHomeEnable && !scanHomeDisable)
            {
                HomeScanHeadAtStart(
                    client,
                    scanMotorPort,
                    scanHomePower,
                    scanHomeSettleMs,
                    scanHomeInvert,
                    scanHomeMaxSweepDegrees,
                    scanHomeSweepStepDegrees,
                    scanHomeStagnantSteps);
            }

            if (maxIterations == 0)
            {
                Console.WriteLine("Seek loop skipped: HADEN_SEEK_MAX_ITERATIONS=0 (scan-home-only semantics).");
                return;
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
            bool hasPreviousCenter = false;
            int previousCenterSmooth = 0;
            int peakLightValue = int.MinValue;
            int peakStableTicks = 0;
            int recoveryEvents = 0;
            bool recoveryPending = false;
            int adaptiveProbePower = Math.Clamp(scanProbePower, 1, 100);
            int adaptiveProbeDegrees = Math.Clamp(scanProbeDegrees, 5, 90);
            int flatProbeCount = 0;
            double minDecisionConfidence = Math.Clamp(minDecisionConfidencePercent / 100.0, 0.0, 1.0);

            while (true)
            {
                if (maxIterations > 0 && completedIterations >= maxIterations)
                {
                    stopReason = "max-iterations";
                    break;
                }

                LightTripletSample triplet = ProbeTriplet(
                    client,
                    lightSensorPort,
                    scanMotorPort,
                    activeLight,
                    scanInvert,
                    adaptiveProbePower,
                    adaptiveProbeDegrees,
                    scanProbeSettleMs);
                int rawSensor = triplet.Center;
                int smoothedSensor = smoother.AddSample(rawSensor);
                int delta = hasPreviousCenter ? (smoothedSensor - previousCenterSmooth) : 0;
                previousCenterSmooth = smoothedSensor;
                hasPreviousCenter = true;

                if (smoothedSensor > peakLightValue)
                {
                    peakLightValue = smoothedSensor;
                    peakStableTicks = 1;
                    if (recoveryPending)
                    {
                        recoveryEvents++;
                        recoveryPending = false;
                    }
                }
                else if (Math.Abs(peakLightValue - smoothedSensor) <= peakTolerance)
                {
                    peakStableTicks++;
                    if (recoveryPending)
                    {
                        recoveryEvents++;
                        recoveryPending = false;
                    }
                }
                else
                {
                    peakStableTicks = 0;
                    recoveryPending = true;
                }

                LightTripletDecision decision = decisionPolicy.Decide(triplet.Left, triplet.Center, triplet.Right);
                bool bumpPressed = ReadBumpPressed(client, bumpSensorPort, bumpActiveLow);
                double reward = LightSeekRewardSignal.Compute(delta, bumpPressed);
                totalReward += reward;

                bool hasDirectionalEvidence =
                    decision.Direction != TurnDirection.None &&
                    decision.Confidence >= minDecisionConfidence;
                if (hasDirectionalEvidence)
                {
                    flatProbeCount = 0;
                    adaptiveProbePower = Math.Clamp(scanProbePower, 1, 100);
                    adaptiveProbeDegrees = Math.Clamp(scanProbeDegrees, 5, 90);
                }
                else
                {
                    flatProbeCount++;
                    if (flatProbeCount >= Math.Max(1, scanProbeFlatLimit))
                    {
                        adaptiveProbePower = Math.Min(Math.Clamp(scanProbePowerMax, 1, 100), adaptiveProbePower + Math.Max(1, scanProbePowerStep));
                        adaptiveProbeDegrees = Math.Min(Math.Clamp(scanProbeDegreesMax, 5, 90), adaptiveProbeDegrees + Math.Max(1, scanProbeDegreesStep));
                        flatProbeCount = 0;
                    }
                }

                int scanDirection = hasDirectionalEvidence
                    ? (decision.Direction == TurnDirection.Left ? -1 : 1)
                    : 0;
                int scanMotorPower = scanDirection == 0 ? 0 : (scanDirection * Math.Clamp(adaptiveProbePower, 1, 100));
                int leftWheelPower = hasDirectionalEvidence ? decision.LeftWheelPower : 0;
                int rightWheelPower = hasDirectionalEvidence ? decision.RightWheelPower : 0;

                if (steerInvert)
                {
                    int swap = leftWheelPower;
                    leftWheelPower = rightWheelPower;
                    rightWheelPower = swap;
                }

                if (!bumpPressed)
                {
                    if (scanDirection != 0)
                    {
                        client.TurnMotor(scanMotorPort, scanMotorPower, Math.Abs(scanProbeDegrees));
                    }
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
                    delta,
                    reward,
                    bumpPressed,
                    scanDirection,
                    scanMotorPower,
                    leftWheelPower,
                    rightWheelPower);

                string stateKey = BuildStateKey(smoothedSensor, delta, bumpPressed);
                string actionKey = BuildActionKey(scanDirection, leftWheelPower, rightWheelPower);
                ScorecardSnapshot scorecard = store.AppendRlPoint(
                    sessionId,
                    stateKey,
                    actionKey,
                    reward,
                    bumpPressed,
                    nowUtc);

                Console.WriteLine(
                    "iter=" + completedIterations +
                    " L=" + triplet.Left +
                    " sensorRaw=" + rawSensor +
                    " sensorSmooth=" + smoothedSensor +
                    " R=" + triplet.Right +
                    " delta=" + delta +
                    " reward=" + reward.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) +
                    " confidence=" + scorecard.Confidence.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture) +
                    " probeConfidence=" + decision.Confidence.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture) +
                    " minProbeConfidence=" + minDecisionConfidence.ToString("0.000", System.Globalization.CultureInfo.InvariantCulture) +
                    " actionMode=" + (hasDirectionalEvidence ? "seek" : "hold") +
                    " adaptiveProbePower=" + adaptiveProbePower +
                    " adaptiveProbeDegrees=" + adaptiveProbeDegrees +
                    " flatProbeCount=" + flatProbeCount +
                    " peak=" + peakLightValue +
                    " stableTicks=" + peakStableTicks +
                    " recoveries=" + recoveryEvents +
                    " bump=" + bumpPressed +
                    " scanDir=" + scanDirection +
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
                peakLightValue == int.MinValue ? 0 : peakLightValue,
                totalReward,
                success);
            Console.WriteLine(
                "Seek complete. reason=" + stopReason +
                ", iterations=" + completedIterations +
                ", totalReward=" + totalReward.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) +
                ", elapsedSec=" + elapsedSeconds.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture));
        }

        private static void RunHomeScanHead(NxtBrickClient client)
        {
            int battery = client.GetBatteryLevel();
            Console.WriteLine("Connected to NXT. Battery mV: " + battery);

            NxtMotorPort scanMotorPort = ReadMotorPortEnv("HADEN_LIGHT_SCAN_MOTOR_PORT", NxtMotorPort.PortA);
            bool scanHomeEnable = ReadBoolEnvAlias("HADEN_SCAN_HOME_ENABLE", "HADEN_CENTER_SCAN_ON_START", true);
            bool scanHomeDisable = ReadBoolEnvAlias("HADEN_SCAN_HOME_DISABLE", "HADEN_CENTER_DISABLE", false);
            bool scanHomeInvert = ReadBoolEnvAlias("HADEN_SCAN_HOME_INVERT", "HADEN_CENTER_INVERT", false);
            int scanHomePower = ReadIntEnvAlias("HADEN_SCAN_HOME_POWER", "HADEN_CENTER_POWER", 22);
            int scanHomeSettleMs = ReadIntEnvAlias("HADEN_SCAN_HOME_SETTLE_MS", "HADEN_CENTER_SETTLE_MS", 250);
            int scanHomeMaxSweepDegrees = ReadIntEnvAlias("HADEN_SCAN_HOME_MAX_SWEEP_DEGREES", "HADEN_CENTER_MAX_SWEEP_DEGREES", 1080);
            int scanHomeSweepStepDegrees = ReadIntEnvAlias("HADEN_SCAN_HOME_SWEEP_STEP_DEGREES", "HADEN_CENTER_SWEEP_STEP_DEGREES", 40);
            int scanHomeStagnantSteps = ReadIntEnvAlias("HADEN_SCAN_HOME_STAGNANT_STEPS", "HADEN_CENTER_STAGNANT_STEPS", 3);

            Console.WriteLine(
                "Home-scan-head setup: scanMotor=" + scanMotorPort +
                ", scanHomeEnable=" + scanHomeEnable +
                ", scanHomeDisable=" + scanHomeDisable +
                ", scanHomeInvert=" + scanHomeInvert);

            if (scanHomeEnable && !scanHomeDisable)
            {
                HomeScanHeadAtStart(
                    client,
                    scanMotorPort,
                    scanHomePower,
                    scanHomeSettleMs,
                    scanHomeInvert,
                    scanHomeMaxSweepDegrees,
                    scanHomeSweepStepDegrees,
                    scanHomeStagnantSteps);
            }

            client.BrakeMotor(scanMotorPort);
            Console.WriteLine("Home-scan-head complete.");
        }

        private static bool ReadBumpPressed(NxtBrickClient client, NxtSensorPort bumpSensorPort, bool activeLow)
        {
            bool pressed = client.ReadTouchSensorPressed(bumpSensorPort);
            return activeLow ? !pressed : pressed;
        }

        private static LightTripletSample ProbeTriplet(
            NxtBrickClient client,
            NxtSensorPort lightSensorPort,
            NxtMotorPort scanMotorPort,
            bool activeLight,
            bool scanInvert,
            int probePower,
            int probeDegrees,
            int settleMs)
        {
            int safePower = Math.Clamp(probePower, 1, 100);
            int safeDegrees = Math.Clamp(probeDegrees, 5, 90);
            int safeSettle = Math.Clamp(settleMs, 20, 2000);
            int leftDirection = scanInvert ? 1 : -1;
            int rightDirection = -leftDirection;

            int center = client.ReadLightSensorValue(lightSensorPort, activeLight);

            client.TurnMotor(scanMotorPort, leftDirection * safePower, safeDegrees);
            System.Threading.Thread.Sleep(safeSettle);
            int left = client.ReadLightSensorValue(lightSensorPort, activeLight);

            client.TurnMotor(scanMotorPort, rightDirection * safePower, safeDegrees * 2);
            System.Threading.Thread.Sleep(safeSettle);
            int right = client.ReadLightSensorValue(lightSensorPort, activeLight);

            client.TurnMotor(scanMotorPort, leftDirection * safePower, safeDegrees);
            System.Threading.Thread.Sleep(safeSettle);
            client.BrakeMotor(scanMotorPort);

            return new LightTripletSample(left, center, right);
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

        private static void HomeScanHeadAtStart(
            NxtBrickClient client,
            NxtMotorPort scanMotorPort,
            int power,
            int settleMs,
            bool invert,
            int maxSweepDegrees,
            int sweepStepDegrees,
            int stagnantSteps)
        {
            int safePower = Math.Clamp(power, 5, 70);
            int settle = Math.Clamp(settleMs, 50, 5000);
            int safeMaxSweep = Math.Clamp(maxSweepDegrees, 180, 4000);
            int safeStep = Math.Clamp(sweepStepDegrees, 10, 180);
            int safeStagnant = Math.Clamp(stagnantSteps, 1, 8);
            int cwDirection = invert ? 1 : -1;
            int ccwDirection = -cwDirection;

            NxtGetOutputState before = client.GetOutputState(scanMotorPort);
            Console.WriteLine(
                "Homing scan head: mode=full-range-sweep" +
                ", power=" + safePower +
                ", settleMs=" + settle +
                ", invert=" + invert +
                ", maxSweepDegrees=" + safeMaxSweep +
                ", sweepStepDegrees=" + safeStep +
                ", rotBefore=" + before.RotationCount);

            // Phase 1: home to CW end stop.
            client.TurnMotor(scanMotorPort, cwDirection * safePower, safeMaxSweep);
            System.Threading.Thread.Sleep(settle);
            client.BrakeMotor(scanMotorPort);
            System.Threading.Thread.Sleep(Math.Min(250, settle));
            client.ResetMotorPosition(scanMotorPort, relative: false);
            NxtGetOutputState afterReset = client.GetOutputState(scanMotorPort);
            Console.WriteLine("Homing scan head: rotAfterReset=" + afterReset.RotationCount);

            // Phase 2: sweep full range toward CCW and detect opposite stop by stagnation.
            int measuredTravel = 0;
            int stagnant = 0;
            int commanded = 0;
            int previousAbs = 0;
            while (commanded < safeMaxSweep && stagnant < safeStagnant)
            {
                int step = Math.Min(safeStep, safeMaxSweep - commanded);
                client.TurnMotor(scanMotorPort, ccwDirection * safePower, step);
                System.Threading.Thread.Sleep(settle);
                client.BrakeMotor(scanMotorPort);
                NxtGetOutputState afterOpposite = client.GetOutputState(scanMotorPort);
                int currentAbs = Math.Abs(afterOpposite.RotationCount);
                measuredTravel = currentAbs;
                commanded += step;

                if (currentAbs <= previousAbs + 2)
                {
                    stagnant++;
                }
                else
                {
                    stagnant = 0;
                }

                previousAbs = Math.Max(previousAbs, currentAbs);
                Console.WriteLine(
                    "Homing scan head: sweepCommanded=" + commanded +
                    ", rotAfterOpposite=" + afterOpposite.RotationCount +
                    ", stagnant=" + stagnant);
            }

            if (Math.Abs(measuredTravel) < 20)
            {
                Console.WriteLine("Homing scan head: insufficient travel detected; leaving motor at current position.");
                client.BrakeMotor(scanMotorPort);
                return;
            }

            // Phase 3: return half the measured travel to midpoint.
            int midpointDegrees = Math.Max(10, measuredTravel / 2);
            client.TurnMotor(scanMotorPort, cwDirection * safePower, midpointDegrees);
            System.Threading.Thread.Sleep(settle);

            client.BrakeMotor(scanMotorPort);
            NxtGetOutputState afterCenter = client.GetOutputState(scanMotorPort);
            Console.WriteLine(
                "Homing scan head: fullTravel=" + measuredTravel +
                ", centerTarget=" + midpointDegrees +
                ", rotAfterCenter=" + afterCenter.RotationCount);
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

        private static int ReadIntEnvAlias(string primaryName, string legacyName, int defaultValue)
        {
            if (HasEnvValue(primaryName))
            {
                return ReadIntEnv(primaryName, defaultValue);
            }

            return ReadIntEnv(legacyName, defaultValue);
        }

        private static bool ReadBoolEnvAlias(string primaryName, string legacyName, bool defaultValue)
        {
            if (HasEnvValue(primaryName))
            {
                return ReadBoolEnv(primaryName, defaultValue);
            }

            return ReadBoolEnv(legacyName, defaultValue);
        }

        private static bool HasEnvValue(string name)
        {
            string raw = Environment.GetEnvironmentVariable(name);
            return !string.IsNullOrWhiteSpace(raw);
        }

        private readonly struct LightTripletSample
        {
            public LightTripletSample(int left, int center, int right)
            {
                Left = left;
                Center = center;
                Right = right;
            }

            public int Left { get; }
            public int Center { get; }
            public int Right { get; }
        }
    }
}
