using Pluspy.Core;
using Pluspy.Net.Packets.Client;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pluspy.Entities
{
    public class ServerModel : JsonSerializable<StatusResponsePacket>
    {
        [JsonPropertyName("version")]
        public ServerVersion Version { get; }

        [JsonPropertyName("players")]
        public ServerPlayerList Players { get; }

        [JsonPropertyName("description")]
        public Text Description { get; }

        [JsonPropertyName("favicon")]
        public string FaviconString { get; }

        public ServerModel(ServerVersion version, ServerPlayerList players,
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
