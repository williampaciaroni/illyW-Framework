using illyW.Framework.EFCore;
using illyW.Framework.EventStore.Entities;
using illyW.Framework.EventStore.Persistence;

namespace illyW.Framework.EventStore.Repositories
{
    public class CheckpointRepository<TContext>(TContext context) : GenericRepository<Checkpoint, string, TContext>(context), ICheckpointRepository where TContext : EventStoreDbContext
    {
    }
    public class StreamCheckpointRepository<TContext>(TContext context) : GenericRepository<StreamCheckpoint, string, TContext>(context), IStreamCheckpointRepository where TContext : EventStoreDbContext
    {
    }
    public class AllCheckpointRepository<TContext>(TContext context) : GenericRepository<AllCheckpoint, string, TContext>(context), IAllCheckpointRepository where TContext : EventStoreDbContext
    {
    }
}
