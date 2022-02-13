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
        private readonly IPlaceRepository _placeRepository;

        public ListPlacesHandler(IPlaceRepository PlaceRepository)
        {
            this._placeRepository = PlaceRepository;
        }

        public async Task<IEnumerable<Place>> HandleAsync(ListPlaces query)
        {
            var Places = await _placeRepository.ListPlacesAsync();

            return Places
                .OrderBy(p => p.Name)
                .ToList();
        }
    }
}
