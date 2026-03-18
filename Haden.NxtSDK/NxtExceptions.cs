using System;

namespace Haden.NxtSDK
{
    public class NxtProtocolException : ApplicationException
    {
        public NxtProtocolException(string message) : base(message)
        {
        }
    }

    public class NxtChannelBusyException : NxtProtocolException
    {
        public NxtChannelBusyException(string message) : base(message)
        {
        }
    }

    public class NxtCommunicationBusErrorException : NxtProtocolException
    {
        public NxtCommunicationBusErrorException(string message) : base(message)
        {
        }
    }
}
