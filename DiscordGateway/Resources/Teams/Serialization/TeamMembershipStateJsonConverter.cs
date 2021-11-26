using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DiscordGateway.Resources.Teams.Serialization
{
    public class TeamMembershipStateJsonConverter
        : JsonConverter<TeamMembershipState>
    {
        public override TeamMembershipState Read(
                ref Utf8JsonReader      reader,
                Type                    typeToConvert,
                JsonSerializerOptions   options)
            => reader.GetString() switch
            {
                "ACCEPTED"      => TeamMembershipState.Accepted,
                "INVITED"       => TeamMembershipState.Invited,
                null            => throw new JsonException($"Invalid value null, for {nameof(TeamMembershipState)}"),
                string value    => throw new JsonException($"Invalid value {value}, for {nameof(TeamMembershipState)}")
            };

        public override void Write(
                Utf8JsonWriter          writer,
                TeamMembershipState     value,
                JsonSerializerOptions   options)
            => writer.WriteStringValue(value switch
            {
                TeamMembershipState.Accepted    => "ACCEPTED",
                TeamMembershipState.Invited     => "INVITED",
                _                               => throw new JsonException($"Invalid value {(int)value}, for {nameof(TeamMembershipState)}")
            });
    }
}
