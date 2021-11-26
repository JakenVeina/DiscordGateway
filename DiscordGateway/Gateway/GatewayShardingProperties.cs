using System.Text.Json.Serialization;

using DiscordGateway.Gateway.Serialization;

namespace DiscordGateway.Gateway
{
    [JsonConverter(typeof(GatewayShardingPropertiesJsonConverter))]
    public class GatewayShardingProperties
    {
        public int ShardCount { get; init; }

        public int ShardId { get; init; }
    }
}
