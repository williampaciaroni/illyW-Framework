using System.Threading;
using System.Threading.Tasks;
using EventStore.Client;
using Microsoft.Extensions.Hosting;

namespace illyW.Framework.EventStore.Rehydrators
{
    public abstract class BaseRehydrator : BackgroundService
    {
        public abstract void HandleEvent(ResolvedEvent resolvedEvent);

        internal abstract IPosition ReadStreamCheckpoint();

        internal abstract Task Subscribe(CancellationToken stoppingToken);

        internal abstract void UpdateCheckpoint(IPosition eventNumber);
    }
}
