using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DiscordGateway.Resources.Serialization
{
    public class OptionalJsonConverter
        : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
            => typeToConvert.IsGenericType
                && (typeToConvert.GetGenericTypeDefinition() == typeof(Optional<>));

        public override JsonConverter? CreateConverter(
                Type typeToConvert,
                JsonSerializerOptions options)
            => (JsonConverter)Activator.CreateInstance(typeof(Converter<>)
                .MakeGenericType(typeToConvert.GetGenericArguments()[0]));

        private class Converter<T>
            : JsonConverter<Optional<T>>
        {
            public override Optional<T> Read(
                    ref Utf8JsonReader reader,
                    Type typeToConvert,
                    JsonSerializerOptions options)
                => JsonSerializer.Deserialize<T>(ref reader, options)!;

            public override void Write(
                Utf8JsonWriter writer,
                Optional<T> value,
                JsonSerializerOptions options)
            {
                if (value.IsSpecified)
                    JsonSerializer.Serialize(writer, value.Value, options);
                else
                    throw new ArgumentException("Unable to serialize an unspecified optional value");
            }
        }
    }
}
