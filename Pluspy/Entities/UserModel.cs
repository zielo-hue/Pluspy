using Pluspy.Net;
using Pluspy.Net.Packets.Client;
using System.Text.Json.Serialization;

namespace Pluspy.Entities
{
    public sealed class UserModel : IPacketModel<LoginSuccessPacket>
    {
        [JsonPropertyName("name")]
        public string Username { get; set; }

        [JsonPropertyName("id")]
        public string UUID { get; set; }

        public UserModel(string username, string uuid)
        {
            Username = username;
            UUID = uuid;
        }

        public LoginSuccessPacket ToPacket()
            => new LoginSuccessPacket(this);
    }
}
