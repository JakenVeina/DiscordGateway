namespace DiscordGateway.Gateway.Payloads
{
    public sealed class HeartbeatAcknowledgementPayload
        : GatewayPayload
    {
        public override GatewayPayloadOpCode OpCode
            => GatewayPayloadOpCode.HeartbeatAcknowledgement;
    }
}
