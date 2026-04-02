using System;
using Haden.NxtSDK;
using Haden.RobotBehavior;

namespace Haden.HardwareSmoke
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            bool seekMaxLightMode = HasFlag(args, "--seek-max-light");
            string port = ResolvePort(args);
            int retries = ReadIntEnv("HADEN_AUTOCONNECT_RETRIES", 5);
            int delayMs = ReadIntEnv("HADEN_AUTOCONNECT_DELAY_MS", 1000);

            Console.WriteLine("Haden hardware smoke starting...");
            Console.WriteLine("Port: " + port);
            Console.WriteLine("Retries: " + retries + ", DelayMs: " + delayMs);
            Console.WriteLine("Mode: " + (seekMaxLightMode ? "seek-max-light" : "smoke"));

            try
            {
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

            NxtSensorPort sensorPort = ReadSensorPortEnv("HADEN_LIGHT_SENSOR_PORT", NxtSensorPort.Port3);
            NxtMotorPort scanMotorPort = ReadMotorPortEnv("HADEN_LIGHT_SCAN_MOTOR_PORT", NxtMotorPort.PortA);
            NxtMotorPort leftWheelPort = ReadMotorPortEnv("HADEN_LEFT_WHEEL_MOTOR_PORT", NxtMotorPort.PortB);
            NxtMotorPort rightWheelPort = ReadMotorPortEnv("HADEN_RIGHT_WHEEL_MOTOR_PORT", NxtMotorPort.PortC);

            int iterations = ReadIntEnv("HADEN_SEEK_ITERATIONS", 30);
            int settleDelayMs = ReadIntEnv("HADEN_SEEK_SETTLE_DELAY_MS", 600);
            bool activeLight = ReadIntEnv("HADEN_LIGHT_SENSOR_ACTIVE", 0) == 1;
            int wheelStepDegrees = ReadIntEnv("HADEN_WHEEL_STEP_DEGREES", 35);
            bool steerInvert = ReadBoolEnv("HADEN_STEER_INVERT", false);
            bool scanInvert = ReadBoolEnv("HADEN_SCAN_INVERT", false);
            int smoothWindow = Math.Clamp(ReadIntEnv("HADEN_LIGHT_SMOOTH_WINDOW", 3), 1, 10);
            var smoother = new LightSignalSmoother(smoothWindow);

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
                "Seek setup: sensor=" + sensorPort +
                ", scanMotor=" + scanMotorPort +
                ", leftWheel=" + leftWheelPort +
                ", rightWheel=" + rightWheelPort +
                ", iterations=" + iterations +
                ", wheelStepDegrees=" + wheelStepDegrees +
                ", active=" + activeLight +
                ", smoothWindow=" + smoothWindow +
                ", steerInvert=" + steerInvert +
                ", scanInvert=" + scanInvert);

            for (int i = 0; i < iterations; i++)
            {
                int rawSensor = client.ReadLightSensorValue(sensorPort, activeLight);
                int smoothedSensor = smoother.AddSample(rawSensor);
                PeakLightSteeringStep step = policy.Advance(smoothedSensor);

                int scanMotorPower = scanInvert ? -step.ScanMotorPower : step.ScanMotorPower;
                int leftWheelPower = step.LeftWheelPower;
                int rightWheelPower = step.RightWheelPower;

                if (steerInvert)
                {
                    int swap = leftWheelPower;
                    leftWheelPower = rightWheelPower;
                    rightWheelPower = swap;
                }

                client.TurnMotor(scanMotorPort, scanMotorPower, Math.Abs(step.ScanDegrees));
                client.TurnMotor(leftWheelPort, leftWheelPower, Math.Abs(wheelStepDegrees));
                client.TurnMotor(rightWheelPort, rightWheelPower, Math.Abs(wheelStepDegrees));

                Console.WriteLine(
                    "iter=" + i +
                    " sensorRaw=" + rawSensor +
                    " sensorSmooth=" + smoothedSensor +
                    " delta=" + step.Delta +
                    " peak=" + step.PeakLightValue +
                    " stableTicks=" + step.PeakStableTicks +
                    " recoveries=" + step.RecoveryEvents +
                    " scanDir=" + step.ScanDirection +
                    " scanPwr=" + scanMotorPower +
                    " leftPwr=" + leftWheelPower +
                    " rightPwr=" + rightWheelPower);

                client.KeepAlive();
                if (settleDelayMs > 0)
                {
                    System.Threading.Thread.Sleep(settleDelayMs);
                }
            }

            client.BrakeMotor(scanMotorPort);
            client.BrakeMotor(leftWheelPort);
            client.BrakeMotor(rightWheelPort);
            Console.WriteLine("Seek complete.");
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
    }
}
