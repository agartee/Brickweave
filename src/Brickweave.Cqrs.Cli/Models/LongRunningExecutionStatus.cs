using System;

namespace Brickweave.Cqrs.Cli.Models
{
    public abstract class LongRunningExecutionStatus
    {
        protected LongRunningExecutionStatus(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }

    public class NotFoundStatus : LongRunningExecutionStatus
    {
        public NotFoundStatus(Guid id) : base(id)
        {
        }
    }

    public class RunningStatus : LongRunningExecutionStatus
    {
        public RunningStatus(Guid id, DateTime started) : base(id)
        {
            Started = started;
        }

        public DateTime Started { get; }
    }

    public class CompletedStatus : LongRunningExecutionStatus
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

    public class ErrorStatus : LongRunningExecutionStatus
    {
        public ErrorStatus(Guid id, DateTime started, DateTime stopped, Exception exception) : base(id)
        {
            Started = started;
            Stopped = stopped;
            Exception = exception;
        }

        public DateTime Started { get; }
        public DateTime Stopped { get; }
        public Exception Exception { get; }
    }
}
