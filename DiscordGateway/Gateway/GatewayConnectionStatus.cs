namespace DiscordGateway.Gateway
{
    public enum GatewayConnectionStatus
    {
        Disconnected    = 0x0100,
        Connecting      = 0x0200,
        Connected       = 0x0300,
        Disconnecting   = 0x0400
    }
}
