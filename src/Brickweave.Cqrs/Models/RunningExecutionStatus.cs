using System;

namespace Brickweave.Cqrs.Models
{
    public class RunningExecutionStatus : ExecutionStatus
    {
        public RunningExecutionStatus(Guid commandId, DateTime start) : base(commandId)
        {
            Start = start;
        }

        public DateTime Start { get; }
    }
}
