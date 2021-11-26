namespace System.Text.Json.Serialization.Converters
{
    internal class UInt16ToStringOrNumberJsonConverter
        : JsonConverter<ushort>
    {
        public override ushort Read(
            ref Utf8JsonReader      reader,
            Type                    typeToConvert,
            JsonSerializerOptions   options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var value = reader.GetString();

                return ushort.TryParse(value, out var result)
                    ? result
                    : throw new JsonException($"Invalid value \"{value}\", for {nameof(UInt16)} value");
            }

            return JsonSerializer.Deserialize<ushort>(ref reader, options);
        }

        public override void Write(
                Utf8JsonWriter          writer,
                ushort                  value,
                JsonSerializerOptions   options)
            => writer.WriteNumberValue(value);
    }
}
