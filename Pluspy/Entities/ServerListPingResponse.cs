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
        public Chat Description { get; }

        [JsonPropertyName("favicon")]
        public string? FaviconString { get; }

        public ServerListPingResponse(ServerListPingResponseVersion version, ServerListPingResponsePlayerList players,
            Chat description, string? faviconString)
        {
            Version = version;
            Players = players;
            Description = description;
            FaviconString = faviconString;
        }
    }
}
