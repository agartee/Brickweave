using System;

namespace Brickweave.Cqrs.Models
{
    public class CommandNotStartedExecutionStatus : ExecutionStatus
    {
        public CommandNotStartedExecutionStatus(Guid commandId) : base(commandId)
        {
        }
    }
}
