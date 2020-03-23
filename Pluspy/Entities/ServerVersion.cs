using System.Text.Json.Serialization;

namespace Pluspy.Entities
{
    public class ServerVersion
    {
        [JsonPropertyName("name")]
        public string Version { get; }
        [JsonPropertyName("protocol")]
        public int Protocol { get; }

        public ServerVersion(string version, int protocol)
        {
            Version = version;
            Protocol = protocol;
        }
    }
}
