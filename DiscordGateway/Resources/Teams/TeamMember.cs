using System.Collections.Generic;
using System.Text.Json.Serialization;

using DiscordGateway.Resources.Users;

namespace DiscordGateway.Resources.Teams
{
    public class TeamMember
    {
        public TeamMember(
            IReadOnlyList<string>   permissios,
            TeamMembershipState     state,
            Snowflake               teamId,
            PartialUser             user)
        {
            Permissions = permissios;
            State       = state;
            TeamId      = teamId;
            User        = user;
        }

        [JsonPropertyName("permissions")]
        public IReadOnlyList<string> Permissions { get; }

        [JsonPropertyName("membership_state")]
        public TeamMembershipState State { get; }

        [JsonPropertyName("team_id")]
        public Snowflake TeamId { get; }

        [JsonPropertyName("user")]
        public PartialUser User { get; }
    }
}
