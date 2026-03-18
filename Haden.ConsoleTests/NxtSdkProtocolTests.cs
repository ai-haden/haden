using System;
using System.Collections.Generic;
using Haden.NxtSDK;
using NUnit.Framework;

namespace Haden.ConsoleTests
{
    [TestFixture]
    public class NxtSdkProtocolTests
    {
        [Test]
        public void ReadLightSensorValue_SendsExpectedCommandsAndParsesScaledValue()
        {
            var transport = new FakeTransport();
            transport.EnqueueReply(Packet(0x02, (byte)NxtCommand.SetInputMode, 0x00));
            transport.EnqueueReply(Packet(
                0x02,
                (byte)NxtCommand.GetInputValues,
                0x00,
                (byte)NxtSensorPort.Port3,
                0x01,
                0x01,
                (byte)NxtSensorType.LightInactive,
                (byte)NxtSensorMode.Percentage,
                0x34,
                0x12,
                0x78,
                0x56,
                0x19,
                0x00,
                0x00,
                0x00,
                0x00));

            var client = new NxtBrickClient(transport);
            client.Connect();

            int value = client.ReadLightSensorValue(NxtSensorPort.Port3, activeLight: false);

            Assert.That(value, Is.EqualTo(25));
            Assert.That(transport.Writes.Count, Is.EqualTo(2));

            byte[] setInputMode = transport.Writes[0];
            Assert.That(setInputMode[2], Is.EqualTo(0x00));
            Assert.That(setInputMode[3], Is.EqualTo((byte)NxtCommand.SetInputMode));
            Assert.That(setInputMode[4], Is.EqualTo((byte)NxtSensorPort.Port3));

            byte[] getInput = transport.Writes[1];
            Assert.That(getInput[3], Is.EqualTo((byte)NxtCommand.GetInputValues));
        }

        [Test]
        public void TurnMotor_EncodesTachoLimitInLittleEndian()
        {
            var transport = new FakeTransport();
            var client = new NxtBrickClient(transport);
            client.Connect();

            client.TurnMotor(NxtMotorPort.PortA, speed: 20, degrees: 90);

            Assert.That(transport.Writes.Count, Is.EqualTo(1));
            byte[] message = transport.Writes[0];

            Assert.That(message[2], Is.EqualTo(0x80));
            Assert.That(message[3], Is.EqualTo((byte)NxtCommand.SetOutputState));
            Assert.That(message[4], Is.EqualTo((byte)NxtMotorPort.PortA));
            Assert.That(message[10], Is.EqualTo(0x5A));
            Assert.That(message[11], Is.EqualTo(0x00));
            Assert.That(message[12], Is.EqualTo(0x00));
            Assert.That(message[13], Is.EqualTo(0x00));
        }

        private static byte[] Packet(params byte[] payload)
        {
            var packet = new byte[payload.Length + 2];
            packet[0] = (byte)(payload.Length & 0xFF);
            packet[1] = (byte)((payload.Length >> 8) & 0xFF);
            Array.Copy(payload, 0, packet, 2, payload.Length);
            return packet;
        }

        private sealed class FakeTransport : INxtTransport
        {
            private readonly Queue<byte> _readBytes = new Queue<byte>();

            public List<byte[]> Writes { get; } = new List<byte[]>();

            public bool IsOpen { get; private set; }

            public int ReadTimeout { get; set; }

            public void Open()
            {
                IsOpen = true;
            }

            public void Close()
            {
                IsOpen = false;
            }

            public void Write(byte[] buffer, int offset, int count)
            {
                var written = new byte[count];
                Array.Copy(buffer, offset, written, 0, count);
                Writes.Add(written);
            }

            public int ReadByte()
            {
                if (_readBytes.Count == 0)
                {
                    return -1;
                }

                return _readBytes.Dequeue();
            }

            public int Read(byte[] buffer, int offset, int count)
            {
                int read = 0;
                while (read < count && _readBytes.Count > 0)
                {
                    buffer[offset + read] = _readBytes.Dequeue();
                    read++;
                }

                return read;
            }

            public void EnqueueReply(byte[] framedPacket)
            {
                foreach (byte b in framedPacket)
                {
                    _readBytes.Enqueue(b);
                }
            }

            public void Dispose()
            {
                Close();
            }
        }
    }
}
