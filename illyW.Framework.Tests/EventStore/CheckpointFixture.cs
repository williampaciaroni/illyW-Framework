using FluentAssertions;
using illyW.Framework.EventStore.Entities;
using illyW.Framework.Tests.Shared.Attributes;

namespace illyW.Framework.Tests.EventStore;

public class CheckpointFixture
{
    [Theory]
    [DefaultAutoData]
    public void StreamCheckpoint_SetPosition_Success(StreamCheckpoint checkpoint)
    {
        var oldPosition = checkpoint.CommitPosition;
        var r = checkpoint.SetCommitPosition(oldPosition + 1);

        r.IsSuccessful.Should().BeTrue();
        checkpoint.CommitPosition.Should().Be(oldPosition + 1);
    }
    
    [Theory]
    [DefaultAutoData]
    public void StreamCheckpoint_SetPosition_Fail(StreamCheckpoint checkpoint)
    {
        var oldPosition = checkpoint.CommitPosition;
        var r = checkpoint.SetCommitPosition(oldPosition - 1);

        r.IsSuccessful.Should().BeFalse();
    }
    
    [Theory]
    [DefaultAutoData]
    public void AllCheckpoint_SetPosition_Success(AllCheckpoint checkpoint)
    {
        var oldCommitPosition = checkpoint.CommitPosition;
        var oldPreparePosition = checkpoint.PreparePosition;
        var r = checkpoint.SetPosition(oldCommitPosition + 1, oldPreparePosition + 1);

        r.IsSuccessful.Should().BeTrue();
        checkpoint.CommitPosition.Should().Be(oldCommitPosition + 1);
        checkpoint.PreparePosition.Should().Be(oldPreparePosition + 1);
    }
    
    [Theory]
    [DefaultAutoData]
    public void AllCheckpoint_SetPosition_CommitPosition_Fail(AllCheckpoint checkpoint)
    {
        var oldCommitPosition = checkpoint.CommitPosition;
        var oldPreparePosition = checkpoint.PreparePosition;
        var r = checkpoint.SetPosition(oldCommitPosition - 1, oldPreparePosition + 1);

        r.IsSuccessful.Should().BeFalse();
        r.Errors.FirstOrDefault().Should().NotBeNull();
        r.Errors.First().Should().StartWith("CommitPosition - New value must be higher than current one.");
    }
    
    [Theory]
    [DefaultAutoData]
    public void AllCheckpoint_SetPosition_PreparePosition_Fail(AllCheckpoint checkpoint)
    {
        var oldCommitPosition = checkpoint.CommitPosition;
        var oldPreparePosition = checkpoint.PreparePosition;
        var r = checkpoint.SetPosition(oldCommitPosition + 1, oldPreparePosition - 1);

        r.IsSuccessful.Should().BeFalse();
        r.Errors.FirstOrDefault().Should().NotBeNull();
        r.Errors.First().Should().StartWith("PreparePosition - New value must be higher than current one.");
    }
}