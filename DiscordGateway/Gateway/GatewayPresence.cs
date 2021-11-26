using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Converters;

namespace DiscordGateway.Gateway
{
    public class GatewayPresence
    {
        public GatewayPresence(
            IReadOnlyList<GatewayPresenceActivity>  activities,
            bool                                    isAfk,
            DateTimeOffset?                         inactiveSince,
            GatewayPresenceStatus                   status)
        {
            Activities      = activities;
            IsAfk           = isAfk;
            InactiveSince   = inactiveSince;
            Status          = status;
        }

        [JsonPropertyName("activities")]
        public IReadOnlyList<GatewayPresenceActivity> Activities { get; }

        [JsonPropertyName("afk")]
        public bool IsAfk { get; }

        [JsonPropertyName("since")]
        [JsonConverter(typeof(DateTimeOffsetToUnixMillisecondsJsonConverter))]
        public DateTimeOffset? InactiveSince { get; }

        [JsonPropertyName("status")]
        public GatewayPresenceStatus Status { get; }
    }
}
