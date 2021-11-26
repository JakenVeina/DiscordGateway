using System.Text.Json.Serialization;

using DiscordGateway.Gateway.Payloads.Serialization;

namespace DiscordGateway.Gateway.Payloads
{
    [JsonConverter(typeof(GatewayPayloadJsonConverter))]
    public abstract class GatewayPayload
    {
        [JsonPropertyName(PropertyNames.OpCode)]
        public abstract GatewayPayloadOpCode OpCode { get; }

        internal static class PropertyNames
        {
            public const string OpCode = "op";
        }
    }
}
