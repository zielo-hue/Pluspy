using Pluspy.Abstraction.Net;
using Pluspy.Net.Packets.Client;
using System.Text.Json.Serialization;

namespace Pluspy.Entities
{
    public sealed class UserModel : IPacketModel<LoginPacket>
    {
        [JsonPropertyName("name")]
        public string Username { get; }

        [JsonPropertyName("id")]
        public string UUID { get; }

        public UserModel(string username, string uuid)
        {
            Username = username;
            UUID = uuid;
        }

        public LoginPacket ToPacket()
            => new LoginPacket(this);
    }
}
