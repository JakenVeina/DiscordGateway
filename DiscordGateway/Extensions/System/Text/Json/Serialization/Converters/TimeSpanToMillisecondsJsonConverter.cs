namespace System.Text.Json.Serialization.Converters
{
    public sealed class TimeSpanToMillisecondsJsonConverter
        : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(
            ref Utf8JsonReader      reader,
            Type                    typeToConvert,
            JsonSerializerOptions   options)
        {
            if (reader.TokenType is not JsonTokenType.Number)
                throw new JsonException($"{nameof(JsonTokenType.Number)} expected, for {nameof(TimeSpan)}(milliseconds) value, encountered {reader.TokenType} instead");

            if (!reader.TryGetInt32(out var value))
                throw new JsonException($"Invalid value ({reader.GetDecimal()}), for {nameof(TimeSpan)}(milliseconds)");

            return TimeSpan.FromMilliseconds(value);
        }

        public override void Write(
                Utf8JsonWriter          writer,
                TimeSpan                value,
                JsonSerializerOptions   options)
            => writer.WriteNumberValue(Convert.ToInt32(value.TotalMilliseconds));
    }
}
