using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pluspy.Entities
{
    public class ServerListPingResponsePlayerList
    {
        [JsonPropertyName("max")]
        public int MaximumPlayers { get; }

        [JsonPropertyName("online")]
        public int OnlinePlayers { get; }

        [JsonPropertyName("sample")]
        public List<UserModel> OnlinePlayerSample { get; }

        public ServerListPingResponsePlayerList(int maxPlayers, int onlinePlayers, List<UserModel> sample)
        {
            MaximumPlayers = maxPlayers;
            OnlinePlayers = onlinePlayers;
            OnlinePlayerSample = sample;
        }
    }
}
