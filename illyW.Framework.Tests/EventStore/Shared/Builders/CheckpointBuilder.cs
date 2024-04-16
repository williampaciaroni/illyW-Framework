using AutoFixture.Kernel;
using illyW.Framework.EventStore.Entities;
using illyW.Framework.Tests.Shared.Builders;

namespace illyW.Framework.Tests.EventStore.Shared.Builders;

public class AllCheckpointBuilder : BaseBuilder<AllCheckpoint, TestEventStoreDbContext>
{
    protected override AllCheckpoint CreateItem(ISpecimenContext context)
    {
        AllCheckpoint allCheckpoint = new AllCheckpoint();

        allCheckpoint.Id = $"all-checkpoint-{Guid.NewGuid()}";

        Random r = new();

        var position = (ulong)r.Next(0, int.MaxValue - 1);

        allCheckpoint.SetPosition(position, position);

        return allCheckpoint;
    }
}

public class StreamCheckpointBuilder : BaseBuilder<StreamCheckpoint, TestEventStoreDbContext>
{
    protected override StreamCheckpoint CreateItem(ISpecimenContext context)
    {
        StreamCheckpoint stremCheckpoint = new StreamCheckpoint();

        stremCheckpoint.Id = $"stream-checkpoint-{Guid.NewGuid()}";

        Random r = new();
        
        stremCheckpoint.SetCommitPosition((ulong)r.Next(0, int.MaxValue - 1));

        return stremCheckpoint;
    }
}