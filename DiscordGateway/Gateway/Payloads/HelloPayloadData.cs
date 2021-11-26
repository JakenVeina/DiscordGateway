using System;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Converters;

namespace DiscordGateway.Gateway.Payloads
{
    public class HelloPayloadData
    {
        [JsonPropertyName("heartbeat_interval")]
        [JsonConverter(typeof(TimeSpanToMillisecondsJsonConverter))]
        public TimeSpan HeartbeatInterval { get; init; }
    }
}
