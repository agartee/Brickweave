using System;

namespace Brickweave.Cqrs.Models
{
    public abstract class ExecutionStatus : IExecutionStatus
    {
        protected ExecutionStatus(Guid commandId)
        {
            CommandId = commandId;
        }

        public Guid CommandId { get; }
    }
}
