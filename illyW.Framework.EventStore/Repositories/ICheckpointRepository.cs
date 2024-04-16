using illyW.Framework.Core.RepositoryPattern;
using illyW.Framework.EventStore.Entities;

namespace illyW.Framework.EventStore.Repositories
{
    public interface ICheckpointRepository : IGenericRepository<Checkpoint, string>
    {
    }
    public interface IStreamCheckpointRepository : IGenericRepository<StreamCheckpoint, string>
    {
    }
    public interface IAllCheckpointRepository : IGenericRepository<AllCheckpoint, string>
    {
    }

}
