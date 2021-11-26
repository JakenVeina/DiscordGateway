using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace DiscordGateway.Gateway.Payloads.Serialization
{
    public interface IGatewayPayloadSerializationFailureHandler
    {
        Task OnSerializationFailed(
            GatewayPayload      payload,
            ReadOnlySpan<byte>  rawPayload,
            int                 rawPayloadMaxLength);

        Task OnDeserializationFailed(
            JsonException       exception,
            ReadOnlySpan<byte>  rawPayload);
    }
}
