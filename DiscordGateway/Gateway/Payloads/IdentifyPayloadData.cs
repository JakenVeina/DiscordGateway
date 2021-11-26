using System.Text.Json.Serialization;

using DiscordGateway.Resources;
using DiscordGateway.Resources.Serialization;

namespace DiscordGateway.Gateway.Payloads
{
    public class IdentifyPayloadData
    {
        public IdentifyPayloadData(
            string                                          authenticationToken,
            GatewayIntents                                  intents,
            Optional<int>                                   offlineGuildMemberThreshold,
            Optional<GatewayPresence>                       presence,
            GatewayConnectionProperties                     properties,      
            Optional<GatewayShardingProperties>   shardingProperties,
            Optional<bool>                                  useCompression)
        {
            AuthenticationToken         = authenticationToken;
            Intents                     = intents;
            OfflineGuildMemberThreshold = offlineGuildMemberThreshold;
            Presence                    = presence;
            Properties                  = properties;
            ShardingProperties          = shardingProperties;
            UseCompression              = useCompression;
        }

        [JsonPropertyName("token")]
        public string AuthenticationToken { get; }

        [JsonPropertyName("intents")]
        public GatewayIntents Intents { get; }

        [JsonPropertyName("large_threshold")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Optional<int> OfflineGuildMemberThreshold { get; }

        [JsonPropertyName("presence")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Optional<GatewayPresence> Presence { get; }
        [JsonPropertyName("properties")]
        public GatewayConnectionProperties Properties { get; }

        [JsonPropertyName("shard")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Optional<GatewayShardingProperties> ShardingProperties { get; }

        [JsonPropertyName("compress")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Optional<bool> UseCompression { get; }
    }
}
