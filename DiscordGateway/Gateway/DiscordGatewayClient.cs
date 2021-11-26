using System;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using DiscordGateway.Gateway.Internal;
using DiscordGateway.Gateway.Payloads.Events;
using DiscordGateway.Gateway.Payloads.Serialization;

namespace DiscordGateway.Gateway
{
    public sealed class DiscordGatewayClient
        : GatewayClientBase
    {
        public static DiscordGatewayClient Create(
                IGatewayDispatchEventReceivedHandler        dispatchEventReceivedHandler,
                IGatewayPayloadSerializationFailureHandler? failureHandler,
                HttpClient                                  httpClient,
                GatewayClientOptions                        options,
                GatewayPresence                             presence)
            => new DiscordGatewayClient(
                dispatchEventReceivedHandler:           dispatchEventReceivedHandler,
                httpClient:                             httpClient,
                options:                                options,
                presence:                               presence,
                randomNumberGenerator:                  new RandomNumberGenerator(),
                socket:                                 new GatewaySocket(failureHandler: failureHandler),
                systemClock:                            new SystemClock());

        internal DiscordGatewayClient(
                IGatewayDispatchEventReceivedHandler    dispatchEventReceivedHandler,
                HttpClient                              httpClient,
                GatewayClientOptions                    options,
                GatewayPresence                         presence,
                IRandomNumberGenerator                  randomNumberGenerator,
                IGatewaySocket                          socket,
                ISystemClock                            systemClock)
            : base(
                dispatchEventReceivedHandler,
                options,
                presence,
                randomNumberGenerator,
                socket,
                systemClock)
        {
            _httpClient = httpClient;
        }

        protected override async Task<string> GetEndpointAsync(CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync("/api/v9/gateway/bot");

            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Gateway Info Retrieved: {content}");

            var info = JsonSerializer.Deserialize<GatewayBotInfo>(
                json:       content,
                options:    null);

            if (info is null)
                throw new GatewayException("Failed to retrieve gateway connection info");

            return info.Url;
        }

        private readonly HttpClient _httpClient;
    }
}
