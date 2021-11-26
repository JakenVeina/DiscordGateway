using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DiscordGateway.Resources.Serialization
{
    public class ImageHashJsonConverter
        : JsonConverter<ImageHash>
    {
        public override ImageHash? Read(
            ref Utf8JsonReader      reader,
            Type                    typeToConvert,
            JsonSerializerOptions   options)
        {
            var value = reader.GetString();
            return (value is null)
                ? null
                : new ImageHash(value);
        }

        public override void Write(
                Utf8JsonWriter          writer,
                ImageHash               value,
                JsonSerializerOptions   options)
            => writer.WriteStringValue(value.Value);
    }
}
