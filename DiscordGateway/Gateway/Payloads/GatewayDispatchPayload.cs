using System.Text.Json.Serialization;

namespace DiscordGateway.Gateway.Payloads
{
    public interface IGatewayDispatchPayload
    {
        object Event { get; }

        GatewayDispatchEventName EventName { get; }

        int SequenceNumber { get; }
    }

    internal class GatewayDispatchPayload
    {
        public static class PropertyNames
        {
            public const string EventName       = "t";
            public const string SequenceNumber  = "s";
        }
    }

    public abstract class GatewayDispatchPayload<TEvent>
            : GatewayDataPayload<TEvent>,
                IGatewayDispatchPayload
        where TEvent : class
    {
        public GatewayDispatchPayload(
                TEvent  @event,
                int     sequenceNumber)
            : base(@event)
        {
            SequenceNumber  = sequenceNumber;
        }

        public override GatewayPayloadOpCode OpCode
            => GatewayPayloadOpCode.Dispatch;

        [JsonIgnore]
        public TEvent Event
            => Data;

        [JsonPropertyName(GatewayDispatchPayload.PropertyNames.EventName)]
        public abstract GatewayDispatchEventName EventName { get; }

        [JsonPropertyName(GatewayDispatchPayload.PropertyNames.SequenceNumber)]
        public int SequenceNumber { get; }

        [JsonIgnore]
        object IGatewayDispatchPayload.Event
            => Event;
    }
}
