using System;

namespace Brickweave.Cqrs.Cli.Models
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
