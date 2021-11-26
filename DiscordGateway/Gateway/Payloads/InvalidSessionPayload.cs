using System.Text.Json.Serialization;

namespace DiscordGateway.Gateway.Payloads
{
    public sealed class InvalidSessionPayload
        : GatewayDataPayload<bool>
    {
        public InvalidSessionPayload(
                bool isResumable)
            : base(isResumable)
        { }

        [JsonIgnore]
        public bool IsResumable
            => Data;

        public override GatewayPayloadOpCode OpCode
            => GatewayPayloadOpCode.InvalidSession;
    }
}
