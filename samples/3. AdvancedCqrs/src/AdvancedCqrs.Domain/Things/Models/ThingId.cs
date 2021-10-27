using System;
using Brickweave.Domain;

namespace AdvancedCqrs.Domain.Things.Models
{
    public class ThingId : Id<Guid>
    {
        public ThingId(Guid value) : base(value)
        {
        }

        public static ThingId NewId()
        {
            return new ThingId(Guid.NewGuid());
        }
    }
}
