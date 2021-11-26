using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Converters;

namespace DiscordGateway.Resources.Users
{
    public class PartialUser
    {
        public PartialUser(
            ImageHash?          avatarHash,
            ushort              discriminator,
            Optional<UserFlags> flags,
            Snowflake           id,
            string              username)
        {
            AvatarHash      = avatarHash;
            Discriminator   = discriminator;
            Flags           = flags;
            Id              = id;
            Username        = username;
        }

        [JsonPropertyName("avatar")]
        public ImageHash? AvatarHash { get; }

        [JsonPropertyName("discriminator")]
        [JsonConverter(typeof(UInt16ToStringOrNumberJsonConverter))]
        public ushort Discriminator { get; }

        [JsonPropertyName("flags")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Optional<UserFlags> Flags { get; }

        [JsonPropertyName("id")]
        public Snowflake Id { get; }

        [JsonPropertyName("username")]
        public string Username { get; }
    }
}
