namespace DiscordGateway.Gateway.Payloads
{
    public sealed class ResumePayload
        : GatewayDataPayload<ResumePayloadData>
    {
        public ResumePayload(ResumePayloadData data)
            : base(data)
        { }

        public override GatewayPayloadOpCode OpCode
            => GatewayPayloadOpCode.Resume;
    }
}
