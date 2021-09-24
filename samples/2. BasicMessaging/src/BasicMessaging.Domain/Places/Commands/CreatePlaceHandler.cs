using System.Threading.Tasks;
using BasicMessaging.Domain.Places.Models;
using BasicMessaging.Domain.Places.Services;
using Brickweave.Cqrs;

namespace BasicMessaging.Domain.Places.Commands
{
    public class CreatePlaceHandler : ICommandHandler<CreatePlace, Place>
    {
        private readonly IPlaceRepository _placeRepository;

        public CreatePlaceHandler(IPlaceRepository placeRepository)
        {
            _placeRepository = placeRepository;
        }

        public async Task<Place> HandleAsync(CreatePlace command)
        {
            var place = new Place(PlaceId.NewId(), command.Name);

            await _placeRepository.SavePlaceAsync(place);

            return place;
        }
    }
}
