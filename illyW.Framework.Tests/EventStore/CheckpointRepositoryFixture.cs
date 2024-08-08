using FluentAssertions;
using illyW.Framework.EventStore.Entities;
using illyW.Framework.EventStore.Repositories;
using illyW.Framework.Tests.EventStore.Shared;
using illyW.Framework.Tests.Shared.Attributes;

namespace illyW.Framework.Tests.EventStore;

public class CheckpointRepositoryFixture
{
    [Theory]
    [DefaultAutoData]
    public void StreamCheckpoint_Create_Success(TestEventStoreDbContext context, StreamCheckpoint streamCheckpoint)
    {
        var repository = CreateStreamCheckpointRepository(context);

        var result = repository.Add(streamCheckpoint);

        result.IsSuccessful.Should().BeTrue();
        context.StreamCheckpoints.SingleOrDefault(x => x.Id == streamCheckpoint.Id).Should().NotBeNull();
    }
    
    [Theory]
    [DefaultAutoData]
    public void StreamCheckpoint_Update_Success(TestEventStoreDbContext context, StreamCheckpoint streamCheckpoint)
    {
        var repository = CreateStreamCheckpointRepository(context);

        var oldPosition = streamCheckpoint.CommitPosition;

        repository.Add(streamCheckpoint);

        streamCheckpoint.SetCommitPosition(oldPosition + 1);
        
        var result = repository.Update(streamCheckpoint);

        result.IsSuccessful.Should().BeTrue();
        context.StreamCheckpoints.SingleOrDefault(x => x.Id == streamCheckpoint.Id).Should().NotBeNull();
        context.StreamCheckpoints.Single(x => x.Id == streamCheckpoint.Id).CommitPosition.Should().Be(oldPosition + 1);
    }
    
    [Theory]
    [DefaultAutoData]
    public void StreamCheckpoint_Delete_Success(TestEventStoreDbContext context, StreamCheckpoint streamCheckpoint)
    {
        var repository = CreateStreamCheckpointRepository(context);

        repository.Add(streamCheckpoint);
        
        var result = repository.Delete(streamCheckpoint);

        result.IsSuccessful.Should().BeTrue();
        context.StreamCheckpoints.SingleOrDefault(x => x.Id == streamCheckpoint.Id).Should().BeNull();
    }
    
    [Theory]
    [DefaultAutoData]
    public void AllCheckpoint_Create_Success(TestEventStoreDbContext context, AllCheckpoint allCheckpoint)
    {
        var repository = CreateAllCheckpointRepository(context);

        var result = repository.Add(allCheckpoint);

        result.IsSuccessful.Should().BeTrue();
        context.AllCheckpoints.SingleOrDefault(x => x.Id == allCheckpoint.Id).Should().NotBeNull();
    }
    
    [Theory]
    [DefaultAutoData]
    public void AllCheckpoint_Update_Success(TestEventStoreDbContext context, AllCheckpoint allCheckpoint)
    {
        var repository = CreateAllCheckpointRepository(context);

        var oldCommitPosition = allCheckpoint.CommitPosition;
        var oldPreparePosition = allCheckpoint.PreparePosition;

        repository.Add(allCheckpoint);

        allCheckpoint.SetPosition(oldCommitPosition + 1, oldPreparePosition + 1);
        
        var result = repository.Update(allCheckpoint);

        result.IsSuccessful.Should().BeTrue();
        context.AllCheckpoints.SingleOrDefault(x => x.Id == allCheckpoint.Id).Should().NotBeNull();
        context.AllCheckpoints.Single(x => x.Id == allCheckpoint.Id).CommitPosition.Should().Be(oldCommitPosition + 1);
        context.AllCheckpoints.Single(x => x.Id == allCheckpoint.Id).PreparePosition.Should().Be(oldPreparePosition + 1);
    }
    
    [Theory]
    [DefaultAutoData]
    public void AllCheckpoint_Delete_Success(TestEventStoreDbContext context, AllCheckpoint allCheckpoint)
    {
        var repository = CreateAllCheckpointRepository(context);

        repository.Add(allCheckpoint);
        
        var result = repository.Delete(allCheckpoint);

        result.IsSuccessful.Should().BeTrue();
        context.AllCheckpoints.SingleOrDefault(x => x.Id == allCheckpoint.Id).Should().BeNull();
    }

    private static StreamCheckpointRepository<TestEventStoreDbContext> CreateStreamCheckpointRepository(TestEventStoreDbContext eventStoreDbContext)
    {
        return new StreamCheckpointRepository<TestEventStoreDbContext>(eventStoreDbContext);
    }
    
    private static AllCheckpointRepository<TestEventStoreDbContext> CreateAllCheckpointRepository(TestEventStoreDbContext eventStoreDbContext)
    {
        return new AllCheckpointRepository<TestEventStoreDbContext>(eventStoreDbContext);
    }
}