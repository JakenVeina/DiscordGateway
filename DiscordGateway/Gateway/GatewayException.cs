using System;
using System.Runtime.Serialization;

namespace DiscordGateway.Gateway
{
    public class GatewayException
        : Exception
    {
        protected GatewayException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

        public GatewayException(string message)
            : base(message)
        { }
    }
}
