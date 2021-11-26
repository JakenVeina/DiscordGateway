namespace DiscordGateway.Gateway
{
    public enum GatewayCloseStatus
    {
        UnknownError            = 4000,
        UnknownOpcode           = 4001,
        DecodeError             = 4002,
        NotAuthenticated        = 4003,
        AuthenticationFailed    = 4004,
        AlreadyAuthenticated    = 4005,
        InvalidSequence         = 4007,
        RateLimited             = 4008,
        SessionTimedOut         = 4009,
        InvalidShard            = 4010,
        ShardingRequired        = 4011,
        InvalidAPIVersion       = 4012,
        InvalidIntents          = 4013,
        DisallowedIntent        = 4014
    }
}
