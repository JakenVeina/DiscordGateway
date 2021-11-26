using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DiscordGateway.Gateway.Serialization
{
    public class GatewayPresenceStatusJsonConverter
        : JsonConverter<GatewayPresenceStatus>
    {
        public override GatewayPresenceStatus Read(
                ref Utf8JsonReader      reader,
                Type                    typeToConvert,
                JsonSerializerOptions   options)
            => reader.GetString() switch
            {
                "dnd"           => GatewayPresenceStatus.DoNotDisturb,
                "idle"          => GatewayPresenceStatus.Idle,
                "invisible"     => GatewayPresenceStatus.Invisible,
                "offline"       => GatewayPresenceStatus.Offline,
                "online"        => GatewayPresenceStatus.Online,
                null            => throw new JsonException($"Invalid value null, for {nameof(GatewayPresenceStatus)}"),
                string value    => throw new JsonException($"Invalid value \"{value}\", for {nameof(GatewayPresenceStatus)}")
            };

        public override void Write(
                Utf8JsonWriter          writer,
                GatewayPresenceStatus   value,
                JsonSerializerOptions   options)
            => writer.WriteStringValue(value switch
            {
                GatewayPresenceStatus.DoNotDisturb  => "dnd",
                GatewayPresenceStatus.Idle          => "idle",
                GatewayPresenceStatus.Invisible     => "invisible",
                GatewayPresenceStatus.Offline       => "offline",
                GatewayPresenceStatus.Online        => "online",
                _                                   => throw new JsonException($"Invalid value {(int)value}, for {nameof(GatewayPresenceStatus)}")
            });
    }
}
