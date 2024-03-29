﻿using System;

namespace Brickweave.Cqrs.Models
{
    public class CommandErrorExecutionStatus : ExecutionStatus
    {
        public CommandErrorExecutionStatus(Guid commandId, DateTime started, DateTime stopped, ExceptionInfo exception) : base(commandId)
        {
            Started = started;
            Stopped = stopped;
            Exception = exception;
        }

        public DateTime Started { get; }
        public DateTime Stopped { get; }
        public ExceptionInfo Exception { get; }
    }
}
