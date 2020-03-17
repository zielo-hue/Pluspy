using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pluspy.Entities
{
    internal class ServerListPingResponsePlayerList
    {
        [JsonPropertyName("max")]
        public int MaximumPlayers { get; }

        [JsonPropertyName("online")]
        public int OnlinePlayers { get; }

        [JsonPropertyName("sample")]
        public List<PlayerListSampleEntry> OnlinePlayerSample { get; }

        public ServerListPingResponsePlayerList(int maxPlayers, int onlinePlayers, List<PlayerListSampleEntry> sample)
        {
            MaximumPlayers = maxPlayers;
            OnlinePlayers = onlinePlayers;
            OnlinePlayerSample = sample;
        }
    }
}
