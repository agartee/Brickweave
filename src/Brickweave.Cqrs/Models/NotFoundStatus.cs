using System;

namespace Brickweave.Cqrs.Models
{
    public class NotFoundStatus : ExecutionStatus
    {
        public NotFoundStatus(Guid commandId) : base(commandId)
        {
        }
    }
}
