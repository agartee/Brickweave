using System;

namespace Brickweave.Cqrs.Cli.Models
{
    public class RunningStatus : ExecutionStatus
    {
        public RunningStatus(Guid commandId, DateTime started) : base(commandId)
        {
            Started = started;
        }

        public DateTime Started { get; }
    }
}
