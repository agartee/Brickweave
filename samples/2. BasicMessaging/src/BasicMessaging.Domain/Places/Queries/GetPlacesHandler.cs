using System.Threading.Tasks;
using BasicMessaging.Domain.Places.Models;
using BasicMessaging.Domain.Places.Services;
using Brickweave.Cqrs;

namespace BasicMessaging.Domain.Places.Queries
{
    public class GetPlacesHandler : IQueryHandler<GetPlace, Place>
    {
        private readonly IPlaceRepository _placeRepository;

        public GetPlacesHandler(IPlaceRepository PlaceRepository)
        {
            _placeRepository = PlaceRepository;
        }

        public async Task<Place> HandleAsync(GetPlace query)
        {
            return await _placeRepository.DemandPlaceAsync(query.Id);
        }
    }
}
