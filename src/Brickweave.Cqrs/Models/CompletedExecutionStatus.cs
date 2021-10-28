using System;

namespace Brickweave.Cqrs.Models
{
    public class CompletedExecutionStatus : ExecutionStatus
    {
        public CompletedExecutionStatus(Guid commandId, DateTime started, DateTime completed, object result) : base(commandId)
        {
            Started = started;
            Completed = completed;
            Result = result;
        }

        public CompletedExecutionStatus(object result) : base(null)
        {
            Result = result;
        }

        public DateTime Started { get; }
        public DateTime Completed { get; }
        public object Result { get; }
    }
}
