using System.Text.Json.Serialization;

namespace DiscordGateway.Gateway.Payloads
{
    internal static class GatewayDataPayload
    {
        public static class PropertyNames
        {
            public const string Data = "d";
        }
    }

    public abstract class GatewayDataPayload<T>
        : GatewayPayload
    {
        public GatewayDataPayload(T data)
            => Data = data;

        [JsonPropertyName(GatewayDataPayload.PropertyNames.Data)]
        public T Data { get; }
    }
}
