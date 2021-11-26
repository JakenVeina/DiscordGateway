using System.Text.Json.Serialization;

namespace DiscordGateway.Resources.Applications
{
    public class PartialApplication
    {
        public PartialApplication(
            Optional<ApplicationFlags>  flags,
            Snowflake                   id)
        {
            Flags   = flags;
            Id      = id;
        }

        [JsonPropertyName("flags")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Optional<ApplicationFlags> Flags { get; }

        [JsonPropertyName("id")]
        public Snowflake Id { get; }
    }
}
