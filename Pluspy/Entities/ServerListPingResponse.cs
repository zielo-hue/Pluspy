using Pluspy.Core;
using Pluspy.Net;
using Pluspy.Net.Packets.Client;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pluspy.Entities
{
    public class ServerListPingResponseModel : JsonSerializable<ServerListPingResponsePacket>
    {
        [JsonPropertyName("version")]
        public ServerListPingResponseVersion Version { get; }

        [JsonPropertyName("players")]
        public ServerListPingResponsePlayerList Players { get; }

        [JsonPropertyName("description")]
        public Text Description { get; }

        [JsonPropertyName("favicon")]
        public string FaviconString { get; }

        public ServerListPingResponseModel(ServerListPingResponseVersion version, ServerListPingResponsePlayerList players,
            Text description, string faviconString)
        {
            Version = version;
            Players = players;
            Description = description;
            FaviconString = faviconString;
            Options = new JsonSerializerOptions 
            { 
                IgnoreNullValues = true
            };
        }
    }
}
