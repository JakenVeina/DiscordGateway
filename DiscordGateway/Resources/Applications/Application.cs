using System.Collections.Generic;
using System.Text.Json.Serialization;

using DiscordGateway.Resources.Teams;
using DiscordGateway.Resources.Users;

namespace DiscordGateway.Resources.Applications
{
    public class Application
        : PartialApplication
    {
        public Application(
            Optional<ImageHash>             coverImageHash,
            string                          description,
            Optional<ApplicationFlags>      flags,
            Optional<Snowflake>             guildId,
            ImageHash?                      iconHash,
            Snowflake                       id,
            bool                            isPublicBot,
            string                          name,
            Optional<PartialUser>           owner,
            Optional<Snowflake>             primarySkuId,
            Optional<string>                privacyPolicyUrl,
            bool                            requiresBotCodeGrant,
            Optional<IReadOnlyList<string>> rpcOrigins,
            Optional<string>                slug,
            string                          summary,
            Team?                           team,
            Optional<string>                termsOfServiceUrl,
            string                          verificationKey)
            : base(
                flags,
                id)
        {
            CoverImageHash          = coverImageHash;
            Description             = description;
            GuildId                 = guildId;
            IconHash                = iconHash;
            IsPublicBot             = isPublicBot;
            Name                    = name;
            Owner                   = owner;
            PrimarySkuId            = primarySkuId;
            PrivacyPolicyUrl        = privacyPolicyUrl;
            RequiresBotCodeGrant    = requiresBotCodeGrant;
            RpcOrigins              = rpcOrigins;
            Slug                    = slug;
            Summary                 = summary;
            Team                    = team;
            TermsOfServiceUrl       = termsOfServiceUrl;
            VerificationKey         = verificationKey;
        }

        [JsonPropertyName("cover_image")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Optional<ImageHash> CoverImageHash { get; }

        [JsonPropertyName("description")]
        public string Description { get; }

        [JsonPropertyName("guild_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Optional<Snowflake> GuildId { get; }

        [JsonPropertyName("icon")]
        public ImageHash? IconHash { get; }

        [JsonPropertyName("bot_public")]
        public bool IsPublicBot { get; }

        [JsonPropertyName("name")]
        public string Name { get; }

        [JsonPropertyName("owner")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Optional<PartialUser> Owner { get; }

        [JsonPropertyName("primary_sku_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Optional<Snowflake> PrimarySkuId { get; }

        [JsonPropertyName("privacy_policy_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Optional<string> PrivacyPolicyUrl { get; }

        [JsonPropertyName("bot_require_code_grant")]
        public bool RequiresBotCodeGrant { get; }

        [JsonPropertyName("rpc_origins")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Optional<IReadOnlyList<string>> RpcOrigins { get; }

        [JsonPropertyName("slug")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Optional<string> Slug { get; }

        [JsonPropertyName("summary")]
        public string Summary { get; }

        [JsonPropertyName("team")]
        public Team? Team { get; }

        [JsonPropertyName("terms_of_service_url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Optional<string> TermsOfServiceUrl { get; }

        [JsonPropertyName("verify_key")]
        public string VerificationKey { get; }
    }
}
