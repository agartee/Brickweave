using System;

namespace Brickweave.Cqrs.Models
{
    public class CompletedStatus : ExecutionStatus
    {
        public CompletedStatus(Guid commandId, DateTime started, DateTime completed, object result) : base(commandId)
        {
            Started = started;
            Completed = completed;
            Result = result;
        }

        public DateTime Started { get; }
        public DateTime Completed { get; }
        public object Result { get; }
    }
}
