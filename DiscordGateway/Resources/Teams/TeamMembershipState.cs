using System.Text.Json.Serialization;

using DiscordGateway.Resources.Teams.Serialization;

namespace DiscordGateway.Resources.Teams
{
    [JsonConverter(typeof(TeamMembershipStateJsonConverter))]
    public enum TeamMembershipState
    {
        Invited     = 1,
        Accepted    = 2
    }
}
