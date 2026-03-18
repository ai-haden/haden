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
    public class HardwareSmokeTests
    {
        private const string ComPortEnvVar = "HADEN_NXT_COM_PORT";

        [Test]
        public void NxtBluetoothSmoke_ConnectReadMoveDisconnect()
        {
            if (!OperatingSystem.IsWindows())
            {
                Assert.Ignore("Hardware smoke tests require Windows.");
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
                for (int i = 0; i < 5; i++)
                {
                    lightSensor.Poll();
                    _ = lightSensor.Value;
                    reads++;
                    Thread.Sleep(100);
                }

                motorA.Turn(20, 90);
                Thread.Sleep(500);
                motorA.Brake();

                Assert.That(reads, Is.EqualTo(5), "Expected to complete the light sensor read loop.");
            }
            finally
            {
                if (connected)
                {
                    brick.Disconnect();
                }
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
                "Set HADEN_NXT_COM_PORT to the NXT Bluetooth COM port to run hardware smoke tests safely.");
            return string.Empty;
        }
    }
}
