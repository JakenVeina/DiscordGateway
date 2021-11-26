namespace DiscordGateway.Gateway.Payloads
{
    public sealed class ReconnectPayload
        : GatewayPayload
    {
        public override GatewayPayloadOpCode OpCode
            => GatewayPayloadOpCode.Reconnect;
    }
}
