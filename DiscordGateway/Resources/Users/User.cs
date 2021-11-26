using System.Text.Json.Serialization;

namespace DiscordGateway.Resources.Users
{
    public class User
        : PartialUser
    {
        public User(
                Optional<int?>                          accentColor,
                ImageHash?                              avatarHash,
                Optional<ImageHash?>                    bannerHash,
                ushort                                  discriminator,
                Optional<string?>                       eMailAddress,
                Optional<UserFlags>                     flags,
                Snowflake                               id,
                Optional<bool>                          isBot,
                Optional<bool>                          isMultiFactorAuthenticationEnabled,
                Optional<bool>                          isSystem,
                Optional<bool>                          isVerified,
                Optional<string>                        locale,
                Optional<UserPremiumSubscriptionType>   premiumSubscriptionType,
                Optional<UserFlags>                     publicFlags,
                string                                  username)
            : base(
                avatarHash,
                discriminator,
                flags,
                id,
                username)
        {
            AccentColor                         = accentColor;
            BannerHash                          = bannerHash;
            EMailAddress                        = eMailAddress;
            IsBot                               = isBot;
            IsMultiFactorAuthenticationEnabled  = isMultiFactorAuthenticationEnabled;
            IsSystem                            = isSystem;
            IsVerified                          = isVerified;
            Locale                              = locale;
            PremiumSubscriptionType             = premiumSubscriptionType;
            PublicFlags                         = publicFlags;
        }

        [JsonPropertyName("accent_color")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Optional<int?> AccentColor { get; }

        [JsonPropertyName("banner")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Optional<ImageHash?> BannerHash { get; }

        [JsonPropertyName("email")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Optional<string?> EMailAddress { get; }

        [JsonPropertyName("bot")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Optional<bool> IsBot { get; }

        [JsonPropertyName("mfa_enabled")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Optional<bool> IsMultiFactorAuthenticationEnabled { get; }

        [JsonPropertyName("system")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Optional<bool> IsSystem { get; }

        [JsonPropertyName("verified")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Optional<bool> IsVerified { get; }

        [JsonPropertyName("locale")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Optional<string> Locale { get; }

        [JsonPropertyName("premium_type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Optional<UserPremiumSubscriptionType> PremiumSubscriptionType { get; }

        [JsonPropertyName("public_flags")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Optional<UserFlags> PublicFlags { get; }
    }
}
