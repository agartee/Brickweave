using System;

namespace Brickweave.Cqrs.Models
{
    public class CommandRunningExecutionStatus : ExecutionStatus
    {
        public CommandRunningExecutionStatus(Guid commandId, DateTime start) : base(commandId)
        {
            Start = start;
        }

        public DateTime Start { get; }
    }
}
