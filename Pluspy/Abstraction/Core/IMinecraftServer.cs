using Pluspy.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Pluspy.Core
{
    public interface IMinecraftServer
    {
        event EventHandler<IMinecraftServer, MinecraftServerEventArgs>? Log;
        int PlayerCapacity { get; }
        Chat Description { get; set; }
        ServerFavicon Icon { get; set; }
        Task StartAsync(CancellationToken token);
        Task StopAsync(CancellationToken token);
    }
}
