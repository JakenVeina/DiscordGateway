using System;
using System.Buffers;
using System.IO;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using DiscordGateway.Gateway.Payloads;
using DiscordGateway.Gateway.Payloads.Serialization;

namespace DiscordGateway.Gateway.Internal
{
    internal interface IGatewaySocket
    {
        Task ConnectAsync(
            Uri                 endpoint,
            CancellationToken   cancellationToken);
        
        Task TryDisconnectAsync(bool willReconnect);

        Task<ReceivePayloadResult> TryReceivePayloadAsync();

        Task<WebSocketCloseStatus?> TrySendPayloadAsync(GatewayPayload payload);

        public readonly struct ReceivePayloadResult
        {
            public WebSocketCloseStatus? CloseStatus { get; init; }

            public GatewayPayload? Payload { get; init; }
        }
    }

    internal class GatewaySocket
        : IGatewaySocket
    {
        public const int RawPayloadMaxLength
            = 4096;

        public GatewaySocket(IGatewayPayloadSerializationFailureHandler? failureHandler)
        {
            _failureHandler = failureHandler;

            _receiveBuffer  = new byte[RawPayloadMaxLength];
            _sendBuffer     = new(RawPayloadMaxLength);
        }

        public async Task ConnectAsync(
            Uri                 endpoint,
            CancellationToken   cancellationToken)
        {
            if (Socket is not null)
                throw new GatewayException("Socket is already connected or connecting");

            var socket = new ClientWebSocket();
            
            await socket.ConnectAsync(endpoint, cancellationToken);

            Socket = socket;
        }

        public async Task TryDisconnectAsync(bool willReconnect)
        {
            var socket = Socket;
            if (socket is null)
                return;

            if (willReconnect)
                await socket.CloseAsync(
                    closeStatus:        WebSocketCloseStatus_ServiceRestart,
                    statusDescription:  "ServiceRestart",
                    cancellationToken:  CancellationToken.None);
            else
                await socket.CloseAsync(
                    closeStatus:        WebSocketCloseStatus.NormalClosure,
                    statusDescription:  nameof(WebSocketCloseStatus.NormalClosure),
                    cancellationToken:  CancellationToken.None);
        }

        public async Task<IGatewaySocket.ReceivePayloadResult> TryReceivePayloadAsync()
        {
            var socket = Socket;
            if (socket is null)
                throw new GatewayException("The socket is disconnecting or disconnected");

            var payloadSize = 0;

            WebSocketReceiveResult result;
            do
            {
                if (payloadSize == _receiveBuffer.Length)
                    Array.Resize(ref _receiveBuffer, _receiveBuffer.Length + 4096);

                var segment = _receiveBuffer.ToSegment(payloadSize, _receiveBuffer.Length - payloadSize);

                result = await socket.ReceiveAsync(segment, CancellationToken.None);
                if (result.CloseStatus.HasValue)
                    return new()
                    {
                        CloseStatus = result.CloseStatus
                    };

                payloadSize += result.Count;
            }
            while (!result.EndOfMessage);

            try
            {
                var payloadText = System.Text.Encoding.UTF8.GetString(_receiveBuffer.ToSegment(0, payloadSize));
                Console.WriteLine($"Payload Received: {payloadText}");

                return new()
                {
                    Payload = JsonSerializer.Deserialize<GatewayPayload>(_receiveBuffer.ToSegment(0, payloadSize))! // Converter prevents the possibility of null
                };
            }
            catch (JsonException exception)
            {
                if (_failureHandler is not null)
                    await _failureHandler.OnDeserializationFailed(
                        exception:  exception,
                        rawPayload: _receiveBuffer.ToSegment(0, payloadSize));

                return default;
            }
        }

        public async Task<WebSocketCloseStatus?> TrySendPayloadAsync(GatewayPayload payload)
        {
            var socket = Socket;
            if (socket is null)
                throw new GatewayException("The socket is disconnecting or disconnected");

            _sendBuffer.Clear();

            using (var writer = new Utf8JsonWriter(_sendBuffer))
                JsonSerializer.Serialize<object>(writer, payload);

            if ((_failureHandler is not null) && (_sendBuffer.WrittenCount > RawPayloadMaxLength))
            {
                await _failureHandler.OnSerializationFailed(
                    payload:                payload,
                    rawPayload:             _sendBuffer.WrittenSpan,
                    rawPayloadMaxLength:    RawPayloadMaxLength);
            }
            else
            {
                try
                {
                    await socket.SendAsync(
                        buffer:             _sendBuffer.WrittenMemory,
                        messageType:        WebSocketMessageType.Text,
                        endOfMessage:       true,
                        cancellationToken:  CancellationToken.None);

                    var payloadText = System.Text.Encoding.UTF8.GetString(_sendBuffer.WrittenSpan);
                    Console.WriteLine($"Payload Sent: {payloadText}");
                }
                catch (Exception) { }
            }

            return socket.CloseStatus;
        }

        protected ClientWebSocket? Socket
        {
            get
            {
                if ((_socket is not null)
                        && (_socket.State is not WebSocketState.CloseReceived
                            and not WebSocketState.CloseSent
                            and not WebSocketState.Open))
                    _socket = null;

                return _socket;
            }
            set => _socket = value;
        }

        // Non-standard close code
        private const WebSocketCloseStatus WebSocketCloseStatus_ServiceRestart
            = (WebSocketCloseStatus)1012;

        private IGatewayPayloadSerializationFailureHandler? _failureHandler;
        private byte[]                                      _receiveBuffer;
        private ArrayBufferWriter<byte>                     _sendBuffer;
        private ClientWebSocket?                            _socket;
    }
}
