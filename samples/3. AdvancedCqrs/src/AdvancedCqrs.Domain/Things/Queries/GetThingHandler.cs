using System.Threading.Tasks;
using AdvancedCqrs.Domain.Things.Models;
using AdvancedCqrs.Domain.Things.Services;
using Brickweave.Cqrs;

namespace AdvancedCqrs.Domain.Things.Queries
{
    public class GetThingHandler : IQueryHandler<GetThing, Thing>
    {
        private readonly IThingRepository _thingRepository;

        public GetThingHandler(IThingRepository thingRepository)
        {
            _thingRepository = thingRepository;
        }

        public async Task<Thing> HandleAsync(GetThing query)
        {
            return await _thingRepository.DemandThingAsync(query.ThingId);
        }
    }
}
