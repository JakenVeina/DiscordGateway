using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DiscordGateway.Resources.Serialization
{
    public class SnowflakeJsonConverter
        : JsonConverter<Snowflake>
    {
        public override Snowflake Read(
            ref Utf8JsonReader      reader,
            Type                    typeToConvert,
            JsonSerializerOptions   options)
        {
            var value = reader.GetString();
            if (value is null)
                throw new JsonException($"Invalid value null, for {nameof(Snowflake)}");

            if (!Snowflake.TryParse(value, out var snowflake))
                throw new JsonException($"Invalid value \"{value}\", for {nameof(Snowflake)}");

            return snowflake;
        }

        public override void Write(
                Utf8JsonWriter          writer,
                Snowflake               value,
                JsonSerializerOptions   options)
            => writer.WriteStringValue(value.ToString());
    }
}
