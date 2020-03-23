using Pluspy.Attributes;
using Pluspy.Entities;
using Pluspy.Enums;
using Pluspy.Net;
using System.Text.Json;

namespace Pluspy.Net.Packets
{
    [Clientbound(State.Status)]
    [Serverbound(State.Status)]
    public struct StatusPingPacket : IPacket
    {
        public long Time { get; private set; }

        public StatusPingPacket(long time)
        {
            Time = time;
        }

        public State ReadFrom(MinecraftStream stream, State state, PacketType type)
        {
            Time = stream.Read<long>();
            return state;
        }

        public readonly State WriteTo(MinecraftStream stream, State state, PacketType type)
        {
            stream.Write(Time);
            return state;
        }
    }
}
