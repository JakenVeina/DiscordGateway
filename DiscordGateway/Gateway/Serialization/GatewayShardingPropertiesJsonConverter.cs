using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DiscordGateway.Gateway.Serialization
{
    public class GatewayShardingPropertiesJsonConverter
        : JsonConverter<GatewayShardingProperties>
    {
        public override GatewayShardingProperties? Read(
            ref Utf8JsonReader      reader,
            Type                    typeToConvert,
            JsonSerializerOptions   options)
        {
            var values = JsonSerializer.Deserialize<int[]>(ref reader, options);
            if (values?.Length is not 2)
                throw new JsonException($"{nameof(GatewayShardingProperties)} array was missing or incomplete: should contain 2 integer values");

            return new GatewayShardingProperties()
            {
                ShardCount  = values[1],
                ShardId     = values[0]
            };
        }

        public override void Write(
                Utf8JsonWriter                      writer,
                GatewayShardingProperties value,
                JsonSerializerOptions               options)
            => JsonSerializer.Serialize(new[] { value.ShardId, value.ShardCount }, options);
    }
}
