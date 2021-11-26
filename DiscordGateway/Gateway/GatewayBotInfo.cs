using System.Text.Json.Serialization;

namespace DiscordGateway.Gateway
{
    public class GatewayBotInfo
        : GatewayInfo
    {
        public GatewayBotInfo(
                GatewaySessionLimiterInfo   sessionLimiterInfo,
                int                         shardCount,
                string                      url)
            : base(url)
        {
            SessionLimiterInfo  = sessionLimiterInfo;
            ShardCount          = shardCount;
        }

        [JsonPropertyName("session_start_limit")]
        public GatewaySessionLimiterInfo SessionLimiterInfo { get; }

        [JsonPropertyName("shards")]
        public int ShardCount { get; }
    }
}
