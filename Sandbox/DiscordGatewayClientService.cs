using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using DiscordGateway.Gateway;

namespace Sandbox
{
    public class DiscordGatewayClientService
        : BackgroundService
    {
        public DiscordGatewayClientService(IConfiguration configuration)
            => _configuration = configuration;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => Console.WriteLine("Shutting down"));

            var botToken = _configuration["BotToken"];

            using var httpClient = new HttpClient()
            {
                BaseAddress = new("https://discord.com")
            };
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                scheme: "Bot",
                parameter: botToken);

            var gatewayClient = DiscordGatewayClient.Create(
                dispatchEventReceivedHandler:           new GatewayDispatchEventReceivedHandler(),
                failureHandler:                         new GatewayPayloadSerializationFailedHandler(),
                httpClient:                             httpClient,
                options:                                new(
                    authenticationToken:            botToken,
                    connectionProperties:           GatewayConnectionProperties.FromLibraryName("Test"),
                    intents:                        GatewayIntents.All,
                    offlineGuildMemberThreshold:    null,
                    shardingProperties:             null,
                    useCompression:                 false),
                presence:                               new(
                    activities:     Array.Empty<GatewayPresenceActivity>(),
                    isAfk:          false,
                    inactiveSince:  null,
                    status:         GatewayPresenceStatus.Online));

            await gatewayClient.RunAsync(stoppingToken);
        }

        private readonly IConfiguration _configuration;
    }
}
