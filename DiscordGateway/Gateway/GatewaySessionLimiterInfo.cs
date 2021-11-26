using System;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Converters;

namespace DiscordGateway.Gateway
{
    public class GatewaySessionLimiterInfo
    {
        [JsonPropertyName("total")]
        public int TotalStarts { get; init; }

        [JsonPropertyName("remaining")]
        public int RemainingStarts { get; init; }

        [JsonPropertyName("reset_after")]
        [JsonConverter(typeof(TimeSpanToMillisecondsJsonConverter))]
        public TimeSpan ResetInterval { get; init; }

        [JsonPropertyName("max_concurrency")]
        public int MaxConcurrency { get; init; }
    }
}
