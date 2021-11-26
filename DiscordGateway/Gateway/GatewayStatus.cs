namespace DiscordGateway.Gateway
{
    public enum GatewayStatus
    {
        Idle                        = GatewayConnectionStatus.Disconnected  + 0x01,
        Failed                      = GatewayConnectionStatus.Disconnected  + 0x02,
        Interrupted                 = GatewayConnectionStatus.Disconnected  + 0x03,
        RetrievingEndpoint          = GatewayConnectionStatus.Connecting    + 0x01,
        EstablishingConnection      = GatewayConnectionStatus.Connecting    + 0x02,
        EstablishingCommunication   = GatewayConnectionStatus.Connected     + 0x01,
        EstablishingSession         = GatewayConnectionStatus.Connected     + 0x02,
        Active                      = GatewayConnectionStatus.Connected     + 0x03,
        ResumingSession             = GatewayConnectionStatus.Connected     + 0x04,
        WaitingToRepairSession      = GatewayConnectionStatus.Connected     + 0x05,
        Disconnecting               = GatewayConnectionStatus.Disconnecting + 0x01,
        Reconnecting                = GatewayConnectionStatus.Disconnecting + 0x02
    }

    public static class GatewayStatusExtensions
    {
        public static GatewayConnectionStatus ToConnectionStatus(this GatewayStatus status)
            => (GatewayConnectionStatus)((int)status & 0xFF00);
    }
}
