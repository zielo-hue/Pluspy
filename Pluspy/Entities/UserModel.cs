using Pluspy.Net;
using System.Text.Json.Serialization;

namespace Pluspy.Entities
{
    public sealed class UserModel 
    {
        [JsonPropertyName("name")]
        public string Username { get; set; }

        [JsonPropertyName("id")]
        public string UUID { get; set; }
    }
}
