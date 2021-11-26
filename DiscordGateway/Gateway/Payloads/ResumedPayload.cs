using DiscordGateway.Gateway.Payloads.Events;

namespace DiscordGateway.Gateway.Payloads
{
    public sealed class ResumedPayload
        : GatewayDispatchPayload<ResumedEvent>
    {
        public ResumedPayload(
                ResumedEvent    @event,
                int             sequenceNumber)
            : base(
                @event,
                sequenceNumber)
        { }

        public override GatewayDispatchEventName EventName
            => GatewayDispatchEventName.Resumed;
    }
}
