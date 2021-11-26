namespace DiscordGateway.Gateway
{
    public record GatewayClientOptions
    {
        public GatewayClientOptions(
            string                      authenticationToken,
            GatewayConnectionProperties connectionProperties,
            GatewayIntents              intents,
            int?                        offlineGuildMemberThreshold,
            GatewayShardingProperties?  shardingProperties,
            bool                        useCompression)
        {
            AuthenticationToken         = authenticationToken;
            ConnectionProperties        = connectionProperties;
            Intents                     = intents;
            OfflineGuildMemberThreshold = offlineGuildMemberThreshold;
            ShardingProperties          = shardingProperties;
            UseCompression              = useCompression;
        }

        public string AuthenticationToken { get; init; }

        public GatewayConnectionProperties ConnectionProperties { get; init; }

        public GatewayIntents Intents { get; init; }

        public int? OfflineGuildMemberThreshold { get; init; }

        public GatewayShardingProperties? ShardingProperties { get; init; }

        public bool UseCompression { get; init; }
    }
}
