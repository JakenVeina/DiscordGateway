using System.Text.Json.Serialization;

namespace DiscordGateway.Resources.Guilds
{
    public class UnavailableGuild
    {
        [JsonPropertyName("id")]
        public Snowflake Id { get; }

        [JsonPropertyName("unavailable")]
        public bool IsUnavailable { get; }
    }
}
