using System.Collections.Generic;
using System.Threading.Tasks;
using AdvancedCqrs.Domain.Things.Models;
using AdvancedCqrs.Domain.Things.Services;
using Brickweave.Cqrs;

namespace AdvancedCqrs.Domain.Things.Queries
{
    public class ListThingsHandler : IQueryHandler<ListThings, IEnumerable<Thing>>
    {
        private readonly IThingRepository _thingRepository;

        public ListThingsHandler(IThingRepository thingRepository)
        {
            _thingRepository = thingRepository;
        }

        public async Task<IEnumerable<Thing>> HandleAsync(ListThings query)
        {
            return await _thingRepository.ListThingsAsync();
        }
    }
}
