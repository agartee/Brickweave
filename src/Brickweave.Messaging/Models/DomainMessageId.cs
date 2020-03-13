using System;
using Brickweave.Domain;

namespace Brickweave.Messaging.Models
{
    public class DomainMessageId : Id<Guid>
    {
        public DomainMessageId(Guid value) : base(value)
        {
        }

        public static DomainMessageId NewId()
        {
            return new DomainMessageId(Guid.NewGuid());
        }
    }
}
