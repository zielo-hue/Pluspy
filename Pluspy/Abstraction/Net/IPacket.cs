using Pluspy.Enums;
using System.Net.Sockets;

namespace Pluspy.Net
{
    public interface IPacket
    {
        State WriteTo(NetworkStream stream, State state, PacketType type);
        State ReadFrom(NetworkStream stream, State state, PacketType type);
    }
}
