using System;

namespace Brickweave.Cqrs.Models
{
    public class NotStartedExecutionStatus : ExecutionStatus
    {
        public NotStartedExecutionStatus(Guid commandId) : base(commandId)
        {
        }
    }
}
