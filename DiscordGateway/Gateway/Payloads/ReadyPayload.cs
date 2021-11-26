using DiscordGateway.Gateway.Payloads.Events;

namespace DiscordGateway.Gateway.Payloads
{
    public sealed class ReadyPayload
        : GatewayDispatchPayload<ReadyEvent>
    {
        public ReadyPayload(
                ReadyEvent  @event,
                int         sequenceNumber)
            : base(
                @event,
                sequenceNumber)
        { }

        public override GatewayDispatchEventName EventName
            => EventName;
    }
}
