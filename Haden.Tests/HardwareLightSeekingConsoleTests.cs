using System;
using System.IO.Ports;
using System.Threading;
using Haden.NxtSharp.Brick;
using Haden.NxtSharp.Motors;
using Haden.NxtSharp.Sensors;
using NUnit.Framework;

namespace Haden.Tests
{
    [TestFixture]
    [Category("Hardware")]
    [NonParallelizable]
    public class HardwareLightSeekingConsoleTests
    {
        private const string ComPortEnvVar = "HADEN_NXT_COM_PORT";

        [Test]
        public void ConsoleLightSeeking_PerformsBoundedSeekCycle()
        {
            if (!OperatingSystem.IsWindows())
            {
                Assert.Ignore("Hardware light-seeking console tests require Windows.");
            }

            string comPort = ResolveComPortOrSkip();

            var brick = new NxtBrick(comPort);
            var motorA = new NxtMotor();
            var lightSensor = new NxtLightSensor { Active = false };
            bool connected = false;

            try
            {
                brick.AutoPoll = false;
                brick.MotorA = motorA;
                brick.Sensor3 = lightSensor;

                motorA.Brick = brick;
                motorA.Port = NxtMotorPort.PortA;

                lightSensor.Brick = brick;
                lightSensor.Port = NxtSensorPort.Port3;

                brick.Connect();
                connected = true;

                int reads = 0;
                int turns = 0;
                int previous = ReadLight(lightSensor, ref reads);
                int currentPower = 15;
                int currentDegrees = 15;
                int direction = 1; // 1 = left, -1 = right

                for (int i = 0; i < 8; i++)
                {
                    Move(motorA, currentPower, currentDegrees, direction);
                    turns++;
                    Thread.Sleep(250);
                    motorA.Brake();

                    int current = ReadLight(lightSensor, ref reads);
                    int delta = current - previous;

                    if (delta > 2)
                    {
                        currentDegrees = Math.Min(currentDegrees + 5, 35);
                    }
                    else
                    {
                        direction *= -1;
                        currentDegrees = Math.Max(currentDegrees - 5, 10);
                    }

                    previous = current;
                }

                Assert.That(reads, Is.GreaterThanOrEqualTo(9), "Expected sensor reads during seek cycle.");
                Assert.That(turns, Is.EqualTo(8), "Expected all bounded seek turns to execute.");
            }
            finally
            {
                if (connected)
                {
                    brick.Disconnect();
                }
            }
        }

        private static int ReadLight(NxtLightSensor lightSensor, ref int reads)
        {
            lightSensor.Poll();
            reads++;
            return lightSensor.Value;
        }

        private static void Move(NxtMotor motorA, int power, int degrees, int direction)
        {
            if (direction > 0)
            {
                motorA.Turn(power, degrees);
            }
            else
            {
                motorA.Turn(-power, degrees);
            }
        }

        private static string ResolveComPortOrSkip()
        {
            string configured = Environment.GetEnvironmentVariable(ComPortEnvVar);
            if (!string.IsNullOrWhiteSpace(configured))
            {
                return configured.Trim();
            }

            string[] ports = SerialPort.GetPortNames();
            if (ports.Length == 0)
            {
                Assert.Ignore(
                    "No COM ports detected. Pair the NXT brick over Bluetooth and expose a serial COM port.");
            }

            Assert.Ignore(
                "Set HADEN_NXT_COM_PORT to the NXT Bluetooth COM port to run hardware light-seeking console tests.");
            return string.Empty;
        }
    }
}
