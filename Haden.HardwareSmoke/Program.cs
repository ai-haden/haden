using System;
using Haden.NxtSDK;

namespace Haden.HardwareSmoke
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            string port = ResolvePort(args);
            int retries = ReadIntEnv("HADEN_AUTOCONNECT_RETRIES", 5);
            int delayMs = ReadIntEnv("HADEN_AUTOCONNECT_DELAY_MS", 1000);

            Console.WriteLine("Haden hardware smoke starting...");
            Console.WriteLine("Port: " + port);
            Console.WriteLine("Retries: " + retries + ", DelayMs: " + delayMs);

            try
            {
                using var client = new NxtBrickClient(port);
                client.ConnectWithRetry(retries, delayMs);
                client.KeepAlive();
                int battery = client.GetBatteryLevel();
                Console.WriteLine("Connected to NXT. Battery mV: " + battery);
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
            if (args != null && args.Length > 0 && !string.IsNullOrWhiteSpace(args[0]))
            {
                return args[0].Trim();
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
    }
}
