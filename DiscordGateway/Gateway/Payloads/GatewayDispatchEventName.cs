using System.Text.Json.Serialization;

using DiscordGateway.Gateway.Payloads.Serialization;

namespace DiscordGateway.Gateway.Payloads
{
    [JsonConverter(typeof(GatewayDispatchEventNameJsonConverter))]
    public enum GatewayDispatchEventName
    {
        Ready,
        Resumed
    }
}
