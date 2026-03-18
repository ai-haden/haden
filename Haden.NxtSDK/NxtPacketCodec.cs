namespace Haden.NxtSDK
{
    internal static class NxtPacketCodec
    {
        public static ushort GetUInt16(byte[] buffer, int start)
        {
            return (ushort)(buffer[start] + (256 * buffer[start + 1]));
        }

        public static short GetInt16(byte[] buffer, int start)
        {
            return (short)(buffer[start] + (256 * buffer[start + 1]));
        }

        public static uint GetUInt32(byte[] buffer, int start)
        {
            uint result = 0;
            result += buffer[start];
            result += (uint)(256 * buffer[start + 1]);
            result += (uint)(65536 * buffer[start + 2]);
            result += (uint)(16777216 * buffer[start + 3]);
            return result;
        }

        public static int GetInt32(byte[] buffer, int start)
        {
            int result = 0;
            result += buffer[start];
            result += 256 * buffer[start + 1];
            result += 65536 * buffer[start + 2];
            result += 16777216 * buffer[start + 3];
            return result;
        }

        public static void SetUInt32(byte[] buffer, int start, uint value)
        {
            for (int i = 0; i < 4; i++)
            {
                buffer[start + i] = (byte)(value & 0xff);
                value /= 256;
            }
        }

        public static void SetInt32(byte[] buffer, int start, int value)
        {
            for (int i = 0; i < 4; i++)
            {
                buffer[start + i] = (byte)(value & 0xff);
                value /= 256;
            }
        }
    }
}
