using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DiscordGateway.Resources.Teams
{
    public class Team
    {
        public Team(
            ImageHash?                  icon,
            Snowflake                   id,
            IReadOnlyList<TeamMember>   members,
            string                      name,
            Snowflake                   ownerId)
        {
            Icon    = icon;
            Id      = id;
            Members = members;
            Name    = name;
            OwnerId = ownerId;
        }

        [JsonPropertyName("icon")]
        public ImageHash? Icon { get; }

        [JsonPropertyName("id")]
        public Snowflake Id { get; }

        [JsonPropertyName("members")]
        public IReadOnlyList<TeamMember> Members { get; }

        [JsonPropertyName("name")]
        public string Name { get; }

        [JsonPropertyName("owner_user_id")]
        public Snowflake OwnerId { get; }
    }
}
