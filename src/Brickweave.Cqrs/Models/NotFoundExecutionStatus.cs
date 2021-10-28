using System;

namespace Brickweave.Cqrs.Models
{
    public class NotFoundExecutionStatus : ExecutionStatus
    {
        public NotFoundExecutionStatus(Guid commandId) : base(commandId)
        {
        }
    }
}
