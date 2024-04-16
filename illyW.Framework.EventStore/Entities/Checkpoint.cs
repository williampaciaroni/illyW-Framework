using System.ComponentModel.DataAnnotations;
using illyW.Framework.Core.GenericEntityPattern;
using illyW.Framework.Core.ResultPattern;

namespace illyW.Framework.EventStore.Entities
{
    public class Checkpoint : IEntity<string>
    {
        [MaxLength(128)]
        public string Id { get; set; }
        public ulong CommitPosition { get; private set; }

        public Result SetCommitPosition(ulong commitPosition)
        {
            Result r = new();
            if (commitPosition < CommitPosition)
            {
                r.Fail();
                r.AddError($"CommitPosition - New value must be higher than current one. Current value: {CommitPosition}. New value: {commitPosition}");
            }
            else
            {
                CommitPosition = commitPosition;
                r.Succeed();
            }

            return r;
        }
    }

    public class StreamCheckpoint : Checkpoint
    {
    }

    public class AllCheckpoint : Checkpoint
    {
        public ulong PreparePosition { get; private set; }
        
        public Result SetPosition(ulong commitPosition, ulong preparePosition)
        {
            Result r = SetCommitPosition(commitPosition);
            
            if (r.IsSuccessful)
            {
                if (preparePosition < PreparePosition)
                {
                    r.Fail();
                    r.AddError($"PreparePosition - New value must be higher than current one. Current value: {PreparePosition}. New value: {preparePosition}");
                }
                else
                {
                    PreparePosition = preparePosition;
                    r.Succeed();
                }
            }

            return r;
        }
    }
}
