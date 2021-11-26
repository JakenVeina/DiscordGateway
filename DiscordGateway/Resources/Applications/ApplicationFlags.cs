using System;

namespace DiscordGateway.Resources.Applications
{
    [Flags]
    public enum ApplicationFlags
    {
        GatewayPresence                 = 0x01000,
        GatewayPresenceLimited          = 0x02000,
        GatewayGuildMembers             = 0x04000,
        GatewayGuildMembersLimited      = 0x08000,
        VerificationPendingGuildLimit   = 0x10000,
        Embedded                        = 0x20000
    }
}
