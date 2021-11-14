using System;

namespace Brickweave.Cqrs.Models
{
    public class CommandNotFoundExecutionStatus : ExecutionStatus
    {
        public CommandNotFoundExecutionStatus(Guid commandId) : base(commandId)
        {
        }
    }
}
