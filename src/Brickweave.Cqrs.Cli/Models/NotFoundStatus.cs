using System;

namespace Brickweave.Cqrs.Cli.Models
{
    public class NotFoundStatus : ExecutionStatus
    {
        public NotFoundStatus(Guid commandId) : base(commandId)
        {
        }
    }
}
