using System.Collections.Generic;
using System.Text.Json.Serialization;

using DiscordGateway.Resources;
using DiscordGateway.Resources.Applications;
using DiscordGateway.Resources.Guilds;
using DiscordGateway.Resources.Users;

namespace DiscordGateway.Gateway.Payloads.Events
{
    public class ReadyEvent
    {
        public ReadyEvent(
            PartialApplication                  application,
            int                                 gatewayVersion,
            IReadOnlyList<UnavailableGuild>     guilds,
            User                                user,
            string                              sessionId,
            Optional<GatewayShardingProperties> shardingProperties)
        {
            Application         = application;
            GatewayVersion      = gatewayVersion;
            Guilds              = guilds;
            User                = user;
            SessionId           = sessionId;
            ShardingProperties  = shardingProperties;
        }

        [JsonPropertyName("application")]
        public PartialApplication Application { get; }

        [JsonPropertyName("v")]
        public int GatewayVersion { get; }

        [JsonPropertyName("guilds")]
        public IReadOnlyList<UnavailableGuild> Guilds { get; }

        [JsonPropertyName("user")]
        public User User { get; }

        [JsonPropertyName("session_id")]
        public string SessionId { get; }

        [JsonPropertyName("shard")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Optional<GatewayShardingProperties> ShardingProperties { get; }
    }
}
