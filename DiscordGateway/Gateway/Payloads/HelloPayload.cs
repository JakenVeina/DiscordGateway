namespace DiscordGateway.Gateway.Payloads
{
    public sealed class HelloPayload
        : GatewayDataPayload<HelloPayloadData>
    {
        public HelloPayload(HelloPayloadData data)
            : base(data)
        { }

        public override GatewayPayloadOpCode OpCode
            => GatewayPayloadOpCode.Hello;
    }
}
