using System.Text.Json.Serialization;

using DiscordGateway.Gateway.Serialization;

namespace DiscordGateway.Gateway
{
    [JsonConverter(typeof(GatewayPresenceStatusJsonConverter))]
    public enum GatewayPresenceStatus
    {
        DoNotDisturb,
        Idle,
        Invisible,
        Offline,
        Online
    }
}
