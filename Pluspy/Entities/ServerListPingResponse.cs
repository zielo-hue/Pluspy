using System.Text.Json.Serialization;

namespace Pluspy.Entities
{
    internal class ServerListPingResponse
    {
        [JsonPropertyName("version")]
        public ServerListPingResponseVersion Version { get; }

        [JsonPropertyName("players")]
        public ServerListPingResponsePlayerList Players { get; }

        [JsonPropertyName("description")]
        public Text Description { get; }

        [JsonPropertyName("favicon")]
        public string? FaviconString { get; }

        public ServerListPingResponse(ServerListPingResponseVersion version, ServerListPingResponsePlayerList players,
            Text description, string? faviconString)
        {
            Version = version;
            Players = players;
            Description = description;
            FaviconString = faviconString;
        }
    }
}
