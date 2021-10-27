using System;

namespace Brickweave.Cqrs.Models
{
    public class ErrorStatus : ExecutionStatus
    {
        public ErrorStatus(Guid commandId, DateTime started, DateTime stopped, string message) : base(commandId)
        {
            Started = started;
            Stopped = stopped;
            Message = message;
        }

        public DateTime Started { get; }
        public DateTime Stopped { get; }
        public string Message { get; }
    }
}
