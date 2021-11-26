using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;

using DiscordGateway.Gateway;

namespace Sandbox
{
    public class DiscordGatewayClientService
        : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => Console.WriteLine("Shutting down"));

            using var httpClient = new HttpClient()
            {
                BaseAddress = new("https://discord.com")
            };
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                scheme: "Bot",
                parameter: "ODYwNzkxOTE3NDA3MjQwMjMy.YOAY8Q.Nn6CTNSMC-2zDDsYqc74BPoQ86w");

            var gatewayClient = DiscordGatewayClient.Create(
                dispatchEventReceivedHandler:           new GatewayDispatchEventReceivedHandler(),
                failureHandler:                         new GatewayPayloadSerializationFailedHandler(),
                httpClient:                             httpClient,
                options:                                new(
                    authenticationToken:            "ODYwNzkxOTE3NDA3MjQwMjMy.YOAY8Q.Nn6CTNSMC-2zDDsYqc74BPoQ86w",
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
    }
}
