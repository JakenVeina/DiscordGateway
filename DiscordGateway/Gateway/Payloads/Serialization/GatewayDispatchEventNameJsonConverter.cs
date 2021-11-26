using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DiscordGateway.Gateway.Payloads.Serialization
{
    public class GatewayDispatchEventNameJsonConverter
        : JsonConverter<GatewayDispatchEventName>
    {
        public override GatewayDispatchEventName Read(
                ref Utf8JsonReader      reader,
                Type                    typeToConvert,
                JsonSerializerOptions   options)
            => reader.GetString() switch
            {
                "READY"         => GatewayDispatchEventName.Ready,
                "RESUMED"       => GatewayDispatchEventName.Resumed,
                null            => throw new JsonException($"Invalid value null, for {nameof(GatewayDispatchEventName)}"),
                string value    => throw new JsonException($"Invalid value \"{value}\", for {nameof(GatewayDispatchEventName)}")
            };

        public override void Write(
                Utf8JsonWriter              writer,
                GatewayDispatchEventName    value,
                JsonSerializerOptions       options)
            => writer.WriteStringValue(value switch
            {
                GatewayDispatchEventName.Ready      => "READY",
                GatewayDispatchEventName.Resumed    => "RESUMED",
                _                                   => throw new JsonException($"Invalid value \"{(int)value}\", for {nameof(GatewayDispatchEventName)}")
            });
    }
}
