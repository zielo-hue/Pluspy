using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Plupsy.Net
{
    public interface IPacket
    {
        void WriteTo(NetworkStream stream);
        ValueTask WriteToAsync(NetworkStream stream, CancellationToken token = default)
        {
            if (token.IsCancellationRequested)
                return new ValueTask(Task.FromCanceled(token));

            WriteTo(stream);
            return default;
        }
    }
}
