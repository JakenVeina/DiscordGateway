using System.Text.Json.Serialization;

namespace DiscordGateway.Gateway.Payloads
{
    public sealed class HeartbeatPayload
        : GatewayDataPayload<int?>
    {
        public HeartbeatPayload(int? lastReceivedSequenceNumber)
            : base(lastReceivedSequenceNumber)
        { }

        public override GatewayPayloadOpCode OpCode
            => GatewayPayloadOpCode.Heartbeat;

        [JsonIgnore]
        public int? LastReceivedSequenceNumber
            => Data;
    }
}
