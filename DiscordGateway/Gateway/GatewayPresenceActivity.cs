using System.Text.Json.Serialization;

namespace DiscordGateway.Gateway
{
    public class GatewayPresenceActivity
    {
        public GatewayPresenceActivity(
            string                      name,
            string?                     streamUrl,
            GatewayPresenceActivityType type)
        {
            Name        = name;
            StreamUrl   = streamUrl;
            Type        = type;
        }

        [JsonPropertyName("name")]
        public string Name { get; }

        [JsonPropertyName("url")]
        public string? StreamUrl { get; }

        [JsonPropertyName("type")]
        public GatewayPresenceActivityType Type { get; }
    }
}
