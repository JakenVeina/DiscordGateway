using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using DiscordGateway.Gateway.Payloads;
using DiscordGateway.Gateway.Payloads.Serialization;

namespace Sandbox
{
    public class GatewayPayloadSerializationFailedHandler
        : IGatewayPayloadSerializationFailureHandler
    {
        public Task OnDeserializationFailed(
            JsonException       exception,
            ReadOnlySpan<byte>  rawPayload)
        {
            Console.WriteLine($"Deserialization Failed: {exception.Message}{Environment.NewLine}\t{Encoding.UTF8.GetString(rawPayload)}");

            return Task.CompletedTask;
        }

        public Task OnSerializationFailed(
            GatewayPayload      payload,
            ReadOnlySpan<byte>  rawPayload,
            int                 rawPayloadMaxLength)
        {
            Console.WriteLine($"Serialization Failed: {Encoding.UTF8.GetString(rawPayload)}");

            return Task.CompletedTask;
        }
    }
}
