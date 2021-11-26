namespace DiscordGateway.Gateway.Payloads
{
    public class IdentifyPayload
        : GatewayDataPayload<IdentifyPayloadData>
    {
        public IdentifyPayload(IdentifyPayloadData data)
            : base(data)
        { }

        public override GatewayPayloadOpCode OpCode
            => GatewayPayloadOpCode.Identify;
    }
}
