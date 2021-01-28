using System;

namespace Brickweave.Cqrs.Cli.Models
{

    public abstract class ExecutionStatus : IExecutionStatus
    {
        protected ExecutionStatus(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }

    public class NotFoundStatus : ExecutionStatus
    {
        public NotFoundStatus(Guid id) : base(id)
        {
        }
    }

    public class RunningStatus : ExecutionStatus
    {
        public RunningStatus(Guid id, DateTime started) : base(id)
        {
            Started = started;
        }

        public DateTime Started { get; }
    }

    public class CompletedStatus : ExecutionStatus
    {
        public CompletedStatus(Guid id, DateTime started, DateTime completed, object result) : base(id)
        {
            Started = started;
            Completed = completed;
            Result = result;
        }

        public DateTime Started { get; }
        public DateTime Completed { get; }
        public object Result { get; }
    }

    public class ErrorStatus : ExecutionStatus
    {
        public ErrorStatus(Guid id, DateTime started, DateTime stopped, string message) : base(id)
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
