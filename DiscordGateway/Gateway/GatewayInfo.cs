using System.Text.Json.Serialization;

namespace DiscordGateway.Gateway
{
    public class GatewayInfo
    {
        public GatewayInfo(string url)
            => Url = url;

        [JsonPropertyName("url")]
        public string Url { get; }
    }
}
