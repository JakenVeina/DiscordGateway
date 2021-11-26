using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using DiscordGateway.Gateway.Payloads.Events;

namespace DiscordGateway.Gateway.Payloads.Serialization
{
    public sealed class GatewayPayloadJsonConverter
        : JsonConverter<GatewayPayload>
    {
        public override GatewayPayload? Read(
            ref Utf8JsonReader      reader,
            Type                    typeToConvert,
            JsonSerializerOptions   options)
        {
            if (reader.TokenType is not JsonTokenType.StartObject)
                throw new JsonException($"{nameof(JsonTokenType.StartObject)} expected, for {nameof(GatewayPayload)}, encountered {reader.TokenType} instead");
            reader.Read();

            var dataReader      = default(Utf8JsonReader);
            var eventName       = default(GatewayDispatchEventName?);
            var opCode          = default(GatewayPayloadOpCode?);
            var sequenceNumber  = default(int?);

            while(reader.TokenType is not JsonTokenType.EndObject)
            {
                if (reader.TokenType is not JsonTokenType.PropertyName)
                {
                    reader.Read();
                    continue;
                }

                var propertyName = reader.GetString();
                reader.Read();

                switch(propertyName)
                {
                    case GatewayDataPayload.PropertyNames.Data:
                        dataReader = reader;
                        reader.Skip();
                        break;

                    case GatewayDispatchPayload.PropertyNames.EventName:
                        eventName = JsonSerializer.Deserialize<GatewayDispatchEventName?>(ref reader, options);
                        break;

                    case GatewayPayload.PropertyNames.OpCode:
                        opCode = JsonSerializer.Deserialize<GatewayPayloadOpCode>(ref reader, options);
                        break;

                    case GatewayDispatchPayload.PropertyNames.SequenceNumber:
                        sequenceNumber = JsonSerializer.Deserialize<int?>(ref reader, options);
                        break;
                }
                reader.Read();
            }

            if (opCode is not GatewayPayloadOpCode opCodeValue)
                throw new JsonException($"\"{GatewayPayload.PropertyNames.OpCode}\" property of {nameof(GatewayPayload)}, was missing");

            switch (opCodeValue)
            {
                case GatewayPayloadOpCode.Dispatch:
                    if (eventName is null)
                        throw new JsonException($"\"{GatewayDispatchPayload.PropertyNames.EventName}\" property of {nameof(GatewayDispatchPayload)}, was missing");

                    if (sequenceNumber is null)
                        throw new JsonException($"\"{GatewayDispatchPayload.PropertyNames.SequenceNumber}\" property of {nameof(GatewayDispatchPayload)}, was missing");

                    if (dataReader.CurrentDepth is 0)
                        throw new JsonException($"\"{GatewayDataPayload.PropertyNames.Data}\" property of {nameof(HeartbeatPayload)}, was missing");

                    switch(eventName.Value)
                    {
                        case GatewayDispatchEventName.Ready:
                            return new ReadyPayload(
                                @event:         JsonSerializer.Deserialize<ReadyEvent>(ref dataReader, options)
                                    ?? throw new JsonException($"invalid value null, for \"{GatewayDataPayload.PropertyNames.Data}\" property of {nameof(ReadyPayload)}"),
                                sequenceNumber: sequenceNumber.Value);

                        case GatewayDispatchEventName.Resumed:
                            return new ResumedPayload(
                                @event:         JsonSerializer.Deserialize<ResumedEvent>(ref dataReader, options)
                                    ?? throw new JsonException($"invalid value null, for \"{GatewayDataPayload.PropertyNames.Data}\" property of {nameof(ResumedPayload)}"),
                                sequenceNumber: sequenceNumber.Value);

                        default:
                            throw new JsonException($"Invalid value {eventName.Value}, for \"{GatewayDispatchPayload.PropertyNames.EventName}\" property of {nameof(GatewayDispatchPayload)}");
                    }

                    throw new NotSupportedException();

                case GatewayPayloadOpCode.Heartbeat:
                    if (dataReader.CurrentDepth is 0)
                        throw new JsonException($"\"{GatewayDataPayload.PropertyNames.Data}\" property of {nameof(HeartbeatPayload)}, was missing");

                    return new HeartbeatPayload(JsonSerializer.Deserialize<int?>(ref dataReader, options));

                case GatewayPayloadOpCode.Identify:
                    if (dataReader.CurrentDepth is 0)
                        throw new JsonException($"\"{GatewayDataPayload.PropertyNames.Data}\" property of {nameof(IdentifyPayload)}, was missing");

                    return new IdentifyPayload(JsonSerializer.Deserialize<IdentifyPayloadData>(ref dataReader, options)
                        ?? throw new JsonException($"invalid value null, for \"{GatewayDataPayload.PropertyNames.Data}\" property of {nameof(IdentifyPayload)}"));

                case GatewayPayloadOpCode.PresenceUpdate:
                    throw new NotSupportedException();

                case GatewayPayloadOpCode.VoiceStateUpdate:
                    throw new NotSupportedException();

                case GatewayPayloadOpCode.Resume:
                    throw new NotSupportedException();

                case GatewayPayloadOpCode.Reconnect:
                    return new ReconnectPayload();

                case GatewayPayloadOpCode.RequestGuildMembers:
                    throw new NotSupportedException();

                case GatewayPayloadOpCode.InvalidSession:
                    if (dataReader.CurrentDepth is 0)
                        throw new JsonException($"\"{GatewayDataPayload.PropertyNames.Data}\" property of {nameof(InvalidSessionPayload)}, was missing");

                    return new InvalidSessionPayload(JsonSerializer.Deserialize<bool>(ref dataReader, options));

                case GatewayPayloadOpCode.Hello:
                    if (dataReader.CurrentDepth is 0)
                        throw new JsonException($"\"{GatewayDataPayload.PropertyNames.Data}\" property of {nameof(GatewayDataPayload)}, was missing");

                    return new HelloPayload(JsonSerializer.Deserialize<HelloPayloadData>(ref dataReader, options)
                        ?? throw new JsonException($"invalid value null, for for \"{GatewayDataPayload.PropertyNames.Data}\" property of {nameof(HelloPayload)}"));

                case GatewayPayloadOpCode.HeartbeatAcknowledgement:
                    return new HeartbeatAcknowledgementPayload();

                default:
                    throw new JsonException($"Invalid value {(int)opCodeValue}, for \"{GatewayPayload.PropertyNames.OpCode}\" property of {nameof(GatewayPayload)}");
            }
        }

        public override void Write(
                Utf8JsonWriter          writer,
                GatewayPayload          value,
                JsonSerializerOptions   options)
            => JsonSerializer.Serialize<object>(writer, value, options);
    }
}
