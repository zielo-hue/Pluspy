using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Pluspy.Entities
{
    public class ServerPlayerList
    {
        [JsonPropertyName("max")]
        public int MaximumPlayers { get; }

        [JsonPropertyName("online")]
        public int OnlinePlayers { get; }

        [JsonPropertyName("sample")]
        public List<UserModel> Players { get; }

        public ServerPlayerList(int maxPlayers, int onlinePlayers, List<UserModel> players)
        {
            MaximumPlayers = maxPlayers;
            OnlinePlayers = onlinePlayers;
            Players = players;
        }
    }
}
