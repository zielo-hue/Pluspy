using Pluspy.Net;
using Pluspy.Utilities;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pluspy.Core
{
    public abstract class JsonSerializable
    {
        [JsonIgnore]
        public JsonSerializerOptions Options { get; protected set; }

        public sealed override string ToString()
            => JsonSerializer.Serialize(this, GetType(), Options);
    }

    public abstract class JsonSerializable<TPacket> : JsonSerializable, IPacketModel<TPacket> where TPacket : IPacket
    {
        public virtual TPacket ToPacket()
            => PacketFactory<TPacket>.FromModel(this);
    }
}
