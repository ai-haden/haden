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
            NxtMotorPort motorPort = ReadMotorPortEnv("HADEN_LIGHT_SEEK_MOTOR_PORT", NxtMotorPort.PortA);
            int iterations = ReadIntEnv("HADEN_SEEK_ITERATIONS", 20);
            int power = ReadIntEnv("HADEN_SEEK_POWER", 25);
            int degrees = ReadIntEnv("HADEN_SEEK_DEGREES", 30);
            int settleDelayMs = ReadIntEnv("HADEN_SEEK_SETTLE_DELAY_MS", 700);
            bool activeLight = ReadIntEnv("HADEN_LIGHT_SENSOR_ACTIVE", 0) == 1;

            var engine = new LegacyAutonomousLightSeekEngine();
            Console.WriteLine(
                "Seek setup: sensor=" + sensorPort +
                ", motor=" + motorPort +
                ", iterations=" + iterations +
                ", power=" + power +
                ", degrees=" + degrees +
                ", active=" + activeLight);

            for (int i = 0; i < iterations; i++)
            {
                int sensor = client.ReadLightSensorValue(sensorPort, activeLight);
                LegacyAutonomousLightSeekStep step = engine.Step(sensor);

                int speed = 0;
                if (step.Turn == TurnDirection.Left)
                {
                    speed = Math.Abs(power);
                }
                else if (step.Turn == TurnDirection.Right)
                {
                    speed = -Math.Abs(power);
                }

                if (speed != 0)
                {
                    client.TurnMotor(motorPort, speed, Math.Abs(degrees));
                }

                Console.WriteLine(
                    "iter=" + i +
                    " sensor=" + sensor +
                    " turn=" + step.Turn +
                    " current=" + step.CurrentValue +
                    " peak=" + step.PeakLightValue +
                    " reward=" + step.Reward);

                client.KeepAlive();
                if (settleDelayMs > 0)
                {
                    System.Threading.Thread.Sleep(settleDelayMs);
                }
            }

            client.BrakeMotor(motorPort);
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
