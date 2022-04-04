using System;
using Brickweave.Domain;

namespace EventSourcing.Domain.Ideas.Models
{
    public class IdeaId : Id<Guid>
    {
        public IdeaId(Guid value) : base(value)
        {
        }

        public static IdeaId NewId()
        {
            return new IdeaId(Guid.NewGuid());
        }
    }
}
