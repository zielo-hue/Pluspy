using System.Text.Json.Serialization;

namespace Pluspy.Entities
{
    public class ServerListPingResponseVersion
    {
        [JsonPropertyName("name")]
        public string ServerVersion { get; }
        [JsonPropertyName("protocol")]
        public int ProtocolVersion { get; }

        public ServerListPingResponseVersion(string serverVersion, int protocolVersion)
        {
            ServerVersion = serverVersion;
            ProtocolVersion = protocolVersion;
        }
    }
}
