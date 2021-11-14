using System;

namespace Brickweave.Cqrs.Models
{
    public class CommandCompletedExecutionStatus : ExecutionStatus
    {
        public CommandCompletedExecutionStatus(Guid commandId, DateTime started, DateTime completed, object result) : base(commandId)
        {
            Started = started;
            Completed = completed;
            Result = result;
        }

        public CommandCompletedExecutionStatus(object result) : base(null)
        {
            Result = result;
        }

        public DateTime Started { get; }
        public DateTime Completed { get; }
        public object Result { get; }
    }
}
