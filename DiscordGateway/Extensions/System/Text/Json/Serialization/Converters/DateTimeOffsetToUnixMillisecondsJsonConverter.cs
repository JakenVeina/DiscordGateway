namespace System.Text.Json.Serialization.Converters
{
    internal class DateTimeOffsetToUnixMillisecondsJsonConverter
        : JsonConverter<DateTimeOffset>
    {
        public override DateTimeOffset Read(
                ref Utf8JsonReader      reader,
                Type                    typeToConvert,
                JsonSerializerOptions   options)
            => DateTimeOffset.FromUnixTimeMilliseconds(reader.GetInt32());

        public override void Write(
                Utf8JsonWriter          writer,
                DateTimeOffset          value,
                JsonSerializerOptions   options)
            => writer.WriteNumberValue(value.ToUnixTimeMilliseconds());
    }
}
