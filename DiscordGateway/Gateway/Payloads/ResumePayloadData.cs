using System.Text.Json.Serialization;

namespace DiscordGateway.Gateway.Payloads
{
    public class ResumePayloadData
    {
        public ResumePayloadData(
            string  authenticationToken,
            int     lastReceivedSequenceNumber,
            string  sessionId)
        {
            AuthenticationToken         = authenticationToken;
            LastReceivedSequenceNumber  = lastReceivedSequenceNumber;
            SessionId = sessionId;
        }

        [JsonPropertyName("token")]
        public string AuthenticationToken { get; }

        [JsonPropertyName("seq")]
        public int LastReceivedSequenceNumber { get; }

        [JsonPropertyName("session_id")]
        public string SessionId { get; }
    }
}
