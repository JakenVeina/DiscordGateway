using System.Text.Json.Serialization;

using DiscordGateway.Resources.Serialization;

namespace DiscordGateway.Resources
{
    [JsonConverter(typeof(ImageHashJsonConverter))]
    public class ImageHash
    {
        public ImageHash(string value)
            => Value = value;

        public bool HasGif
            => Value.StartsWith("a_");

        public string Value { get; }
    }
}
