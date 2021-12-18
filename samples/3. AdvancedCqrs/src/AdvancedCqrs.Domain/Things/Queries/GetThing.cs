using AdvancedCqrs.Domain.Things.Models;
using Brickweave.Cqrs;

namespace AdvancedCqrs.Domain.Things.Queries
{
    public class GetThing : IQuery<Thing>
    {
        public GetThing(ThingId thingId)
        {
            ThingId = thingId;
        }

        public ThingId ThingId { get; }
    }
}
