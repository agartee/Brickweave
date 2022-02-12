using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasicMessaging.Domain.Places.Models;
using BasicMessaging.Domain.Places.Services;
using Brickweave.Cqrs;

namespace BasicMessaging.Domain.Places.Queries
{
    public class ListPlacesHandler : IQueryHandler<ListPlaces, IEnumerable<Place>>
    {
        private readonly IPlaceRepository PlaceRepository;

        public ListPlacesHandler(IPlaceRepository PlaceRepository)
        {
            this.PlaceRepository = PlaceRepository;
        }

        public async Task<IEnumerable<Place>> HandleAsync(ListPlaces query)
        {
            var Places = await PlaceRepository.ListPlacesAsync();

            return Places
                .OrderBy(p => p.Name)
                .ToList();
        }
    }
}
