using System;
using System.Text;

namespace Haden.NxtSDK
{
    public sealed class NxtBrickClient : IDisposable
    {
        private const int DefaultI2CStatusPollLimit = 20;
        private readonly INxtTransport _transport;
        private readonly bool _ownsTransport;

        public NxtBrickClient(string portName)
            : this(new SerialPortNxtTransport(portName), ownsTransport: true)
        {
        }

        public NxtBrickClient(INxtTransport transport, bool ownsTransport = false)
        {
            _transport = transport ?? throw new ArgumentNullException(nameof(transport));
            _ownsTransport = ownsTransport;
            _transport.ReadTimeout = 5000;
        }

        public bool IsConnected => _transport.IsOpen;

        public void Connect()
        {
            if (!_transport.IsOpen)
            {
                _transport.Open();
            }
        }

        public void Disconnect()
        {
            if (_transport.IsOpen)
            {
                _transport.Close();
            }
        }

        public void Dispose()
        {
            Disconnect();
            if (_ownsTransport)
            {
                _transport.Dispose();
            }
        }

        public void KeepAlive()
        {
            SendMessage(new byte[] { 0x80, (byte)NxtCommand.KeepAlive });
        }

        public void SetBrickName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (name.Length > 14)
            {
                name = name.Substring(0, 14);
            }

            byte[] nameBytes = Encoding.ASCII.GetBytes(name);
            byte[] message = new byte[18];
            message[0] = 0x81;
            message[1] = (byte)NxtCommand.SetBrickName;
            Array.Copy(nameBytes, 0, message, 2, nameBytes.Length);
            SendMessage(message);
        }

        public int GetBatteryLevel()
        {
            byte[] reply = SendMessage(new byte[] { 0x00, (byte)NxtCommand.GetBatteryLevel });
            return NxtPacketCodec.GetUInt16(reply, 3);
        }

        public void SetInputMode(NxtSensorPort port, NxtSensorType type, NxtSensorMode mode)
        {
            byte[] message = new byte[5];
            message[0] = 0x00;
            message[1] = (byte)NxtCommand.SetInputMode;
            message[2] = (byte)port;
            message[3] = (byte)type;
            message[4] = (byte)mode;
            SendMessage(message);
        }

        public NxtGetInputValues GetInputValues(NxtSensorPort port)
        {
            byte[] message = new byte[3];
            message[0] = 0x00;
            message[1] = (byte)NxtCommand.GetInputValues;
            message[2] = (byte)port;
            byte[] reply = SendMessage(message);

            var result = new NxtGetInputValues
            {
                Valid = reply[4] == 1,
                Calibrated = reply[5] == 1,
                Type = (NxtSensorType)reply[6],
                Mode = (NxtSensorMode)reply[7],
                RawAd = NxtPacketCodec.GetUInt16(reply, 8),
                NormalizedAd = NxtPacketCodec.GetUInt16(reply, 10),
                ScaledValue = NxtPacketCodec.GetInt16(reply, 12),
                CalibratedValue = NxtPacketCodec.GetInt16(reply, 14)
            };
            return result;
        }

        public int ReadLightSensorValue(NxtSensorPort port, bool activeLight = false)
        {
            SetInputMode(
                port,
                activeLight ? NxtSensorType.LightActive : NxtSensorType.LightInactive,
                NxtSensorMode.Percentage);

            return GetInputValues(port).ScaledValue;
        }

        public void SetOutputState(
            NxtMotorPort port,
            sbyte power,
            NxtMotorMode mode,
            NxtMotorRegulationMode regulationMode,
            sbyte turnRatio,
            NxtMotorRunState runState,
            uint tachoLimit)
        {
            byte[] message = new byte[12];
            message[0] = 0x80;
            message[1] = (byte)NxtCommand.SetOutputState;
            message[2] = (byte)port;
            message[3] = (byte)power;
            message[4] = (byte)mode;
            message[5] = (byte)regulationMode;
            message[6] = (byte)turnRatio;
            message[7] = (byte)runState;
            NxtPacketCodec.SetUInt32(message, 8, tachoLimit);
            SendMessage(message);
        }

        public NxtGetOutputState GetOutputState(NxtMotorPort port)
        {
            byte[] message = new byte[3];
            message[0] = 0x00;
            message[1] = (byte)NxtCommand.GetOutputState;
            message[2] = (byte)port;
            byte[] reply = SendMessage(message);

            var result = new NxtGetOutputState
            {
                Power = (sbyte)reply[4],
                Mode = (NxtMotorMode)reply[5],
                RegulationMode = (NxtMotorRegulationMode)reply[6],
                TurnRatio = (sbyte)reply[7],
                RunState = (NxtMotorRunState)reply[8],
                TachoLimit = NxtPacketCodec.GetUInt32(reply, 9),
                TachoCount = NxtPacketCodec.GetInt32(reply, 13),
                BlockTachoCount = NxtPacketCodec.GetInt32(reply, 17),
                RotationCount = NxtPacketCodec.GetInt32(reply, 21)
            };
            return result;
        }

        public void TurnMotor(NxtMotorPort port, int speed, int degrees)
        {
            if (speed < -100 || speed > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(speed), "Speed must be between -100 and 100.");
            }

            if (degrees < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(degrees), "Degrees must be zero or positive.");
            }

            SetOutputState(
                port,
                (sbyte)speed,
                NxtMotorMode.MotorOn | NxtMotorMode.Regulated,
                NxtMotorRegulationMode.MotorSpeed,
                0,
                NxtMotorRunState.Running,
                (uint)degrees);
        }

        public void BrakeMotor(NxtMotorPort port)
        {
            SetOutputState(
                port,
                0,
                NxtMotorMode.MotorOn | NxtMotorMode.Brake | NxtMotorMode.Regulated,
                NxtMotorRegulationMode.MotorSpeed,
                0,
                NxtMotorRunState.Running,
                0);
        }

        public void ResetMotorPosition(NxtMotorPort port, bool relative)
        {
            byte[] message = new byte[4];
            message[0] = 0x80;
            message[1] = (byte)NxtCommand.ResetMotorPosition;
            message[2] = (byte)port;
            message[3] = relative ? (byte)1 : (byte)0;
            SendMessage(message);
        }

        public int LsGetStatus(NxtSensorPort port)
        {
            byte[] message = new byte[3];
            message[0] = 0x00;
            message[1] = (byte)NxtCommand.LsGetStatus;
            message[2] = (byte)port;
            byte[] reply = SendMessage(message);
            return reply[3];
        }

        public void LsWrite(NxtSensorPort port, byte[] data, int returnMessageLength)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.Length < 1 || data.Length > 16)
            {
                throw new InvalidOperationException("The data length must be between 1 and 16 bytes.");
            }

            byte[] message = new byte[5 + data.Length];
            message[0] = 0x00;
            message[1] = (byte)NxtCommand.LsWrite;
            message[2] = (byte)port;
            message[3] = (byte)data.Length;
            message[4] = (byte)returnMessageLength;
            Array.Copy(data, 0, message, 5, data.Length);
            SendMessage(message);
        }

        public byte[] LsRead(NxtSensorPort port)
        {
            byte[] message = new byte[3];
            message[0] = 0x00;
            message[1] = (byte)NxtCommand.LsRead;
            message[2] = (byte)port;
            byte[] reply = SendMessage(message);
            int length = reply[3];
            byte[] result = new byte[length];
            Array.Copy(reply, 4, result, 0, length);
            return result;
        }

        public void MessageWrite(byte mailbox, byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.Length > 57)
            {
                throw new ArgumentException("Data size must be less than 58 bytes.", nameof(data));
            }

            byte[] message = new byte[5 + data.Length];
            message[0] = 0x80;
            message[1] = (byte)NxtCommand.MessageWrite;
            message[2] = mailbox;
            message[3] = (byte)(data.Length + 1);
            Array.Copy(data, 0, message, 4, data.Length);
            SendMessage(message);
        }

        public void MessageWrite(byte mailbox, string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            MessageWrite(mailbox, Encoding.ASCII.GetBytes(value));
        }

        public void MessageWrite(byte mailbox, int value)
        {
            byte[] data = new byte[4];
            NxtPacketCodec.SetInt32(data, 0, value);
            MessageWrite(mailbox, data);
        }

        public void MessageWrite(byte mailbox, bool value)
        {
            MessageWrite(mailbox, new[] { value ? (byte)0x01 : (byte)0x00 });
        }

        public byte[] MessageRead(byte mailbox)
        {
            byte[] message = new byte[5];
            message[0] = 0x00;
            message[1] = (byte)NxtCommand.MessageRead;
            message[2] = (byte)(mailbox + 10);
            message[3] = (byte)(mailbox + 10);
            message[4] = 0x01;
            byte[] reply = SendMessage(message);
            int size = reply[4];
            byte[] result = new byte[size];
            Array.Copy(reply, 5, result, 0, size);
            return result;
        }

        public string MessageReadString(byte mailbox)
        {
            byte[] data = MessageRead(mailbox);
            return Encoding.ASCII.GetString(data, 0, data.Length - 1);
        }

        public int MessageReadInt(byte mailbox)
        {
            byte[] data = MessageRead(mailbox);
            return NxtPacketCodec.GetInt32(data, 0);
        }

        public bool MessageReadBool(byte mailbox)
        {
            byte[] data = MessageRead(mailbox);
            return data[0] != 0;
        }

        public void I2CSetByte(NxtSensorPort port, byte address, byte value)
        {
            byte[] command = new byte[3];
            command[0] = 0x02;
            command[1] = address;
            command[2] = value;
            LsWrite(port, command, 0);
        }

        public byte I2CGetByte(NxtSensorPort port, byte address)
        {
            byte[] command = new byte[2];
            command[0] = 0x02;
            command[1] = address;

            int bytesRead = 0;
            for (int i = 0; i < DefaultI2CStatusPollLimit && bytesRead < 1; i++)
            {
                LsWrite(port, command, 1);
                try
                {
                    bytesRead = LsGetStatus(port);
                }
                catch (NxtCommunicationBusErrorException)
                {
                    bytesRead = 0;
                }
            }

            if (bytesRead < 1)
            {
                throw new NxtProtocolException("I2C read status did not report available bytes.");
            }

            return LsRead(port)[0];
        }

        private byte[] SendMessage(byte[] message)
        {
            if (!_transport.IsOpen)
            {
                throw new InvalidOperationException("Not connected to a serial transport.");
            }

            byte[] framed = new byte[message.Length + 2];
            int length = message.Length;
            framed[0] = (byte)(length & 0xFF);
            framed[1] = (byte)((length & 0xFF00) >> 8);
            Array.Copy(message, 0, framed, 2, message.Length);

            _transport.Write(framed, 0, framed.Length);

            if (message[0] >= 0x80)
            {
                return null;
            }

            int lsb = _transport.ReadByte();
            int msb = _transport.ReadByte();
            if (lsb < 0 || msb < 0)
            {
                throw new NxtProtocolException("Unexpected end of stream while reading reply length.");
            }

            int responseLength = lsb + (msb * 256);
            byte[] reply = new byte[responseLength];
            ReadExactly(reply, 0, responseLength);

            ValidateReply(message, reply);
            return reply;
        }

        private void ReadExactly(byte[] buffer, int offset, int count)
        {
            int totalRead = 0;
            while (totalRead < count)
            {
                int read = _transport.Read(buffer, offset + totalRead, count - totalRead);
                if (read <= 0)
                {
                    throw new NxtProtocolException("Unexpected end of stream while reading NXT response.");
                }

                totalRead += read;
            }
        }

        private static void ValidateReply(byte[] request, byte[] reply)
        {
            if (reply.Length < 3)
            {
                throw new NxtProtocolException("Reply packet too short.");
            }

            if (reply[0] != 0x02)
            {
                throw new NxtProtocolException("Unexpected reply packet type.");
            }

            if (reply[1] != request[1])
            {
                throw new NxtProtocolException(
                    "Reply command mismatch. Expected 0x"
                    + request[1].ToString("x2")
                    + ", got 0x"
                    + reply[1].ToString("x2")
                    + ".");
            }

            byte status = reply[2];
            if (status == 0)
            {
                return;
            }

            string message = "Brick error status 0x" + status.ToString("x2") + ".";
            if ((NxtMessageResult)status == NxtMessageResult.ChannelBusy)
            {
                throw new NxtChannelBusyException(message);
            }

            if ((NxtMessageResult)status == NxtMessageResult.CommBusError)
            {
                throw new NxtCommunicationBusErrorException(message);
            }

            throw new NxtProtocolException(message);
        }
    }
}
