using System;
using System.IO.Ports;

namespace Haden.NxtSDK
{
    public interface INxtTransport : IDisposable
    {
        bool IsOpen { get; }
        int ReadTimeout { get; set; }
        void Open();
        void Close();
        void Write(byte[] buffer, int offset, int count);
        int ReadByte();
        int Read(byte[] buffer, int offset, int count);
    }

    public sealed class SerialPortNxtTransport : INxtTransport
    {
        private readonly SerialPort _port;

        public SerialPortNxtTransport(string portName, int baudRate = 115200)
        {
            if (string.IsNullOrWhiteSpace(portName))
            {
                throw new ArgumentException("Port name is required.", nameof(portName));
            }

            _port = new SerialPort(portName, baudRate);
        }

        public bool IsOpen => _port.IsOpen;

        public int ReadTimeout
        {
            get => _port.ReadTimeout;
            set => _port.ReadTimeout = value;
        }

        public void Open()
        {
            _port.Open();
        }

        public void Close()
        {
            if (_port.IsOpen)
            {
                _port.Close();
            }
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            _port.Write(buffer, offset, count);
        }

        public int ReadByte()
        {
            return _port.ReadByte();
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            return _port.Read(buffer, offset, count);
        }

        public void Dispose()
        {
            Close();
            _port.Dispose();
        }
    }
}
