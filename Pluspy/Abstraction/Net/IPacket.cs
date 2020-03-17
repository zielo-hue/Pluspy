using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Pluspy.Net
{
    public interface IPacket
    {
        void WriteTo(NetworkStream stream);
    }
}
