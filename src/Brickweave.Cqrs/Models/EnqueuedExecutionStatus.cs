using System;

namespace Brickweave.Cqrs.Models
{
    public class EnqueuedExecutionStatus : ExecutionStatus
    {
        public EnqueuedExecutionStatus(Guid commandId) : base(commandId)
        {
        }
    }
}
