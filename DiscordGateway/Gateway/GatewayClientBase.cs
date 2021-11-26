using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

using DiscordGateway.Gateway.Internal;
using DiscordGateway.Gateway.Payloads;
using DiscordGateway.Gateway.Payloads.Events;
using DiscordGateway.Resources;

namespace DiscordGateway.Gateway
{
    public interface IGatewayClient
    {
        GatewayPresence Presence { get; }

        GatewayStatus Status { get; }

        Task RunAsync(CancellationToken stopToken);
    }

    public abstract class GatewayClientBase
        : IGatewayClient
    {
        internal GatewayClientBase(
            IGatewayDispatchEventReceivedHandler    dispatchEventReceivedHandler,
            GatewayClientOptions                    options,
            GatewayPresence                         presence,
            IRandomNumberGenerator                  randomNumberGenerator,
            IGatewaySocket                          socket,
            ISystemClock                            systemClock)
        {
            _dispatchEventReceivedHandler   = dispatchEventReceivedHandler;
            _options                        = options;
            _presence                       = presence;
            _randomNumberGenerator          = randomNumberGenerator;
            _socket                         = socket;
            _systemClock                    = systemClock;

            _incomingConnectionPayloads = CreateChannel<GatewayPayload>(singleWriter: true);
            _incomingHeartbeatPayloads  = CreateChannel<GatewayPayload>(singleWriter: true);
            _incomingSessionPayloads    = CreateChannel<GatewayPayload>(singleWriter: true);
            _outgoingConsumerPayloads   = CreateChannel<GatewayPayload>(singleWriter: false);
            _outgoingHeartbeatPayloads  = CreateChannel<GatewayPayload>(singleWriter: true);
            _outgoingSessionPayloads    = CreateChannel<GatewayPayload>(singleWriter: false);
            _receivedDispatchEvents     = CreateChannel<object>(singleWriter: true);

            _status = GatewayStatus.Idle;
        }

        public GatewayPresence Presence
            => _presence;

        public GatewayStatus Status
            => _status;

        public async Task RunAsync(CancellationToken stopToken)
        {
            if (_status.ToConnectionStatus() is not GatewayConnectionStatus.Disconnected)
                throw new InvalidOperationException("The client is already running");

            try
            {
                while (!stopToken.IsCancellationRequested)
                {
                    _status = GatewayStatus.RetrievingEndpoint;
                    Console.WriteLine($"Status Changed: {_status}");
                
                    var endpoint = await GetEndpointAsync(stopToken);

                    _status = GatewayStatus.EstablishingConnection;
                    Console.WriteLine($"Status Changed: {_status}");

                    if (!Uri.TryCreate(endpoint + "?v=9&encoding=json", UriKind.Absolute, out var endpointUri))
                        throw new GatewayException($"Invalid gateway endpoint retrieved: \"{endpoint}\"");
                    await _socket.ConnectAsync(endpointUri, stopToken);

                    var result = await RunManagementOperationsAsync(
                        new ManagementOperation[]
                        {
                            ManageConnectionAsync,
                            ManageDisconnectionAsync,
                            ManageIncomingPayloadsAsync
                        },
                        stopToken);

                    if (result is ManagementResult.ConnectionLost)
                    {
                        _status = GatewayStatus.Interrupted;
                        Console.WriteLine($"Status Changed: {_status}");
                    }
                    else
                    {
                        _status = (result is ManagementResult.ReconnectRequested)
                            ? GatewayStatus.Reconnecting
                            : GatewayStatus.Disconnecting;
                        Console.WriteLine($"Status Changed: {_status}");

                        await _socket.TryDisconnectAsync(willReconnect: result is not ManagementResult.Success);
                    }

                    if (result is not ManagementResult.Success)
                        continue;
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception)
            {
                _status = GatewayStatus.Failed;
                Console.WriteLine($"Status Changed: {_status}");
                throw;
            }
            finally
            {
                FlushChannel(_outgoingConsumerPayloads);
                FlushChannel(_receivedDispatchEvents);
            }

            _status = GatewayStatus.Idle;
            Console.WriteLine($"Status Changed: {_status}");
        }

        protected abstract Task<string> GetEndpointAsync(CancellationToken cancellationToken);

        private async Task<ManagementResult> ManageConnectionAsync(CancellationToken stopToken)
        {
            try
            {
                _status = GatewayStatus.EstablishingCommunication;
                Console.WriteLine($"Status Changed: {_status}");

                var firstPayload = await _incomingConnectionPayloads.Reader.ReadAsync(stopToken);
                if (firstPayload is not HelloPayload hello)
                    throw new GatewayException($"Unable to establish or re-establish communication: An unexpected payload ({firstPayload.OpCode}) was received, instead of {nameof(GatewayPayloadOpCode.Hello)}.");

                _heartbeatInterval = hello.Data.HeartbeatInterval;

                var result = await RunManagementOperationsAsync(
                    new ManagementOperation[]
                    {
                        ManageHeartbeatAsync,
                        ManageOutgoingPayloadsAsync,
                        ManageReceivedDispatchEvents,
                        ManageSessionAsync
                    },
                    stopToken);

                return result;
            }
            finally
            {
                FlushChannel(_incomingConnectionPayloads);
            }
        }

        private async Task<ManagementResult> ManageDisconnectionAsync(CancellationToken stopToken)
        {
            var stopSource = new TaskCompletionSource<object?>();

            stopToken.Register(() => stopSource.SetResult(null));

            await stopSource.Task;

            await _socket.TryDisconnectAsync(willReconnect: false);

            return ManagementResult.Success;
        }

        private async Task<ManagementResult> ManageHeartbeatAsync(CancellationToken stopToken)
        {
            try
            {
                var heartbeatInterval = _heartbeatInterval!.Value;
                await Task.Delay(heartbeatInterval, stopToken);

                while (!stopToken.IsCancellationRequested)
                {
                    HEARTBEAT_INTERVAL_START:
                    var intervalStart = DateTimeOffset.Now;

                    await _outgoingHeartbeatPayloads.Writer.WriteAsync(
                        new HeartbeatPayload(_lastReceivedSequenceNumber),
                        stopToken);

                    var acknowledged = false;
                    while (!stopToken.IsCancellationRequested)
                    {
                        while (_incomingHeartbeatPayloads.Reader.TryRead(out var payload))
                        {
                            switch(payload)
                            {
                                case HeartbeatAcknowledgementPayload:
                                    acknowledged = true;
                                    break;

                                case HeartbeatPayload:
                                    goto HEARTBEAT_INTERVAL_START;
                            }
                        }

                        var remainingInterval = heartbeatInterval - (DateTimeOffset.Now - intervalStart);
                        if (remainingInterval < TimeSpan.Zero)
                        {
                            if (acknowledged)
                                break;
                            else
                                return ManagementResult.HeartbeatTimeout;
                        }

                        await await Task.WhenAny(
                            _systemClock.SleepAsync(remainingInterval, stopToken),
                            _incomingHeartbeatPayloads.Reader.WaitToReadAsync(stopToken).AsTask());
                    }
                }
            }
            finally
            {
                FlushChannel(_incomingHeartbeatPayloads);
            }

            return ManagementResult.Success;
        }

        private async Task<ManagementResult> ManageIncomingPayloadsAsync(CancellationToken stopToken)
        {
            while (!stopToken.IsCancellationRequested)
            {
                var result = await _socket.TryReceivePayloadAsync();

                if (result.CloseStatus is not null)
                    return ManagementResult.ConnectionLost;

                if (result.Payload is null)
                    continue;

                switch(result.Payload.OpCode)
                {
                    case GatewayPayloadOpCode.Heartbeat:
                    case GatewayPayloadOpCode.HeartbeatAcknowledgement:
                        await _incomingHeartbeatPayloads.Writer.WriteAsync(result.Payload, stopToken);
                        break;

                    case GatewayPayloadOpCode.Dispatch:
                    case GatewayPayloadOpCode.Reconnect:
                    case GatewayPayloadOpCode.InvalidSession:
                        await _incomingSessionPayloads.Writer.WriteAsync(result.Payload, stopToken);
                        break;

                    case GatewayPayloadOpCode.Hello:
                        await _incomingConnectionPayloads.Writer.WriteAsync(result.Payload, stopToken);
                        break;
                }
            }

            return ManagementResult.Success;
        }

        private async Task<ManagementResult> ManageOutgoingPayloadsAsync(CancellationToken stopToken)
        {
            try
            {
                while (!stopToken.IsCancellationRequested)
                {
                    while (_outgoingHeartbeatPayloads.Reader.TryRead(out var payload)
                        || _outgoingSessionPayloads.Reader.TryRead(out payload)
                        || _outgoingConsumerPayloads.Reader.TryRead(out payload))
                    {
                        var closeCode = await _socket.TrySendPayloadAsync(payload);
                        if (closeCode is not null)
                            return ManagementResult.ConnectionLost;
                    }

                    await Task.WhenAny(
                        _outgoingConsumerPayloads.Reader.WaitToReadAsync(stopToken).AsTask(),
                        _outgoingHeartbeatPayloads.Reader.WaitToReadAsync(stopToken).AsTask(),
                        _outgoingSessionPayloads.Reader.WaitToReadAsync(stopToken).AsTask());
                }
            }
            finally
            {
                FlushChannel(_outgoingHeartbeatPayloads);
                FlushChannel(_outgoingSessionPayloads);
            }

            return ManagementResult.Success;
        }

        private async Task<ManagementResult> ManageReceivedDispatchEvents(CancellationToken stopToken)
        {
            var dispatches = new List<Task>();

            while(!stopToken.IsCancellationRequested || (dispatches.Count is not 0))
            {
                var waits = dispatches.AsEnumerable();

                if (!stopToken.IsCancellationRequested)
                {
                    while (_receivedDispatchEvents.Reader.TryRead(out var @event))
                    {
                        var dispatch = _dispatchEventReceivedHandler.OnDispatchEventReceived(@event, stopToken);

                        if (!dispatch.IsCompleted)
                            dispatches.Add(dispatch);
                    }

                    waits = waits.Append(_receivedDispatchEvents.Reader.WaitToReadAsync(stopToken).AsTask());
                }

                try
                {
                    await Task.WhenAny(waits);
                }
                catch (OperationCanceledException) { }

                dispatches.RemoveAll(static dispatch => dispatch.IsCompleted);
            }

            return ManagementResult.Success;
        }

        private async Task<ManagementResult> ManageSessionAsync(CancellationToken stopToken)
        {
            try
            {
                while(!stopToken.IsCancellationRequested)
                {
                    SESSION_MANAGEMENT_START:
                    if (_sessionId is null || _lastReceivedSequenceNumber is null)
                    {
                        _status = GatewayStatus.EstablishingSession;
                        Console.WriteLine($"Status Changed: {_status}");

                        await _outgoingSessionPayloads.Writer.WriteAsync(
                            new IdentifyPayload(new(
                                authenticationToken:            _options.AuthenticationToken,
                                intents:                        _options.Intents,
                                offlineGuildMemberThreshold:    _options.OfflineGuildMemberThreshold ?? default(Optional<int>),
                                presence:                       _presence,
                                properties:                     _options.ConnectionProperties,
                                shardingProperties:             _options.ShardingProperties ?? default(Optional<GatewayShardingProperties>),
                                useCompression:                 _options.UseCompression)),
                            stopToken);
                    }
                    else
                    {
                        _status = GatewayStatus.ResumingSession;
                        Console.WriteLine($"Status Changed: {_status}");

                        await _outgoingSessionPayloads.Writer.WriteAsync(
                            new ResumePayload(new(
                                authenticationToken:            _options.AuthenticationToken,
                                lastReceivedSequenceNumber:     _lastReceivedSequenceNumber.Value,
                                sessionId:                      _sessionId)),
                            stopToken);
                    }

                    while (!stopToken.IsCancellationRequested)
                    {
                        var payload = await _incomingSessionPayloads.Reader.ReadAsync(stopToken);
                        switch(payload)
                        {
                            case IGatewayDispatchPayload dispatch:
                                _lastReceivedSequenceNumber = dispatch.SequenceNumber;

                                if (dispatch is ReadyPayload ready)
                                {
                                    _sessionId = ready.Data.SessionId;
                                    _status = GatewayStatus.Active;
                                    Console.WriteLine($"Status Changed: {_status}");
                                }

                                if (dispatch is ResumedPayload)
                                {
                                    _status = GatewayStatus.Active;
                                    Console.WriteLine($"Status Changed: {_status}");
                                }

                                await _receivedDispatchEvents.Writer.WriteAsync(dispatch.Event, stopToken);

                                break;

                            case ReconnectPayload:
                                return ManagementResult.ReconnectRequested;

                            case InvalidSessionPayload invalidSession:
                                if (!invalidSession.IsResumable)
                                {
                                    _lastReceivedSequenceNumber = null;
                                    _sessionId                  = null;
                                }

                                _status = GatewayStatus.WaitingToRepairSession;
                                Console.WriteLine($"Status Changed: {_status}");

                                await _systemClock.SleepAsync(TimeSpan.FromSeconds(_randomNumberGenerator.Next(1, 6)), stopToken);
                            
                                goto SESSION_MANAGEMENT_START;
                        }
                    }
                }
            }
            finally
            {
                FlushChannel(_incomingSessionPayloads);
            }

            return ManagementResult.Success;
        }

        private async Task<ManagementResult> RunManagementOperationsAsync(
            IEnumerable<ManagementOperation>    operations,
            CancellationToken                   stopToken)
        {
            using var stopConnectionTokenSource = CancellationTokenSource.CreateLinkedTokenSource(stopToken);

            var tasks = operations
                .Select(operation => RunManagementOperationAsync(operation))
                .ToArray();

            await Task.WhenAny(tasks);

            stopConnectionTokenSource.Cancel();
            
            var result = (await Task.WhenAll(tasks))
                .Where(result => result is not ManagementResult.Success)
                .FirstOrDefault();

            return result;

            async Task<ManagementResult> RunManagementOperationAsync(ManagementOperation operation)
            {
                try
                {
                    return await operation.Invoke(stopConnectionTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    return ManagementResult.Success;
                }
            }
        }

        private static Channel<T> CreateChannel<T>(bool singleWriter)
            => Channel.CreateUnbounded<T>(new UnboundedChannelOptions()
            {
                AllowSynchronousContinuations   = true,
                SingleReader                    = true,
                SingleWriter                    = singleWriter,
            });

        private static void FlushChannel<T>(Channel<T> channel)
        {
            while (channel.Reader.TryRead(out _)) ;
        }

        private readonly IGatewayDispatchEventReceivedHandler   _dispatchEventReceivedHandler;
        private readonly Channel<GatewayPayload>                _incomingConnectionPayloads;
        private readonly Channel<GatewayPayload>                _incomingHeartbeatPayloads;
        private readonly Channel<GatewayPayload>                _incomingSessionPayloads;
        private readonly GatewayClientOptions                   _options;
        private readonly Channel<GatewayPayload>                _outgoingConsumerPayloads;
        private readonly Channel<GatewayPayload>                _outgoingHeartbeatPayloads;
        private readonly Channel<GatewayPayload>                _outgoingSessionPayloads;
        private readonly IRandomNumberGenerator                 _randomNumberGenerator;
        private readonly Channel<object>                        _receivedDispatchEvents;
        private readonly IGatewaySocket                         _socket;
        private readonly ISystemClock                           _systemClock;

        private TimeSpan?       _heartbeatInterval;
        private int?            _lastReceivedSequenceNumber;
        private GatewayPresence _presence;
        private string?         _sessionId;
        private GatewayStatus   _status;

        private delegate Task<ManagementResult> ManagementOperation(CancellationToken stopToken);

        private enum ManagementResult
        {
            Success,
            HeartbeatTimeout,
            ConnectionLost,
            ReconnectRequested
        }
    }
}
