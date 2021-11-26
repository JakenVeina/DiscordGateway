using System.Runtime.Serialization;

namespace DiscordGateway.Gateway
{
    public class GatewayDisconnectedException
        : GatewayException
    {
        protected GatewayDisconnectedException(
                    SerializationInfo   info,
                    StreamingContext    context)
                : base(info, context)
            => CloseStatus = (int)info.GetValue(nameof(CloseStatus), typeof(int));

        public GatewayDisconnectedException(
                    int     closeStatus,
                    string  closeStatusDescription)
                : base($"The gateway has disconnected: {closeStatusDescription}")
            => CloseStatus = closeStatus;

        public int CloseStatus { get; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue(nameof(CloseStatus), CloseStatus, typeof(int));
        }
    }
}
