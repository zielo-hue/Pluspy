using Pluspy.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pluspy.Net.Packets.Play.Requests
{
    public struct PlayerPositionAndLookRequest : IRequestPacket
    {
        public byte Id => 0x36;

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public byte Flags { get; set; }
        public int TeleportId { get; set; }

        public State WriteTo(MinecraftStream stream, State state)
        {
            stream.Write(X);
            stream.Write(Y);
            stream.Write(Z);
            stream.Write(Yaw);
            stream.Write(Pitch);
            stream.WriteByte(Flags);
            stream.WriteVarInt(TeleportId);
            return state;
        }
    }
}
