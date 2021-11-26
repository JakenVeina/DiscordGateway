using System;

namespace DiscordGateway.Resources.Users
{
    [Flags]
    public enum UserFlags
    {
        None                        = 0x00000,
        DiscordEmployee             = 0x00001,
        PartneredServerOwner        = 0x00002,
        HypeSquadEvents             = 0x00004,
        BugHunterLevel1             = 0x00008,
        HouseBravery                = 0x00040,
        HouseBrilliance             = 0x00080,
        HouseBalance                = 0x00100,
        EarlySupporter              = 0x00200,
        TeamUser                    = 0x00400,
        BugHunterLevel2             = 0x04000,
        VerifiedBot                 = 0x10000,
        EarlyVerifiedBotDeveloper   = 0x20000,
        DiscordCertifiedModerator   = 0x40000
    }
}
