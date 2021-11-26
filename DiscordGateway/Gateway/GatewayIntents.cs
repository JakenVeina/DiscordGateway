using System;

namespace DiscordGateway.Gateway
{
    [Flags]
    public enum GatewayIntents
    {
        None                    = 0x0000,
        Guilds                  = 0x0001,
        GuildMembers            = 0x0002,
        GuildBans               = 0x0004,
        GuildEmojisAndStickers  = 0x0008,
        GuildIntegrations       = 0x0010,
        GuildWebhooks           = 0x0020,
        GuildInvites            = 0x0040,
        GuildVoiceStates        = 0x0080,
        GuildPresences          = 0x0100,
        GuildMessages           = 0x0200,
        GuildMessageReactions   = 0x0400,
        GuildMessageTyping      = 0x0800,
        DirectMessages          = 0x1000,
        DirectMessageReactions  = 0x2000,
        DirectMessageTyping     = 0x4000,
        All                     = 0x7FFF
    }
}
