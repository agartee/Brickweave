using System.Threading.Tasks;
using BasicMessaging.Domain.Places.Models;
using BasicMessaging.Domain.Places.Services;
using Brickweave.Cqrs;

namespace BasicMessaging.Domain.Places.Commands
{
    public class UpdatePlaceHandler : ICommandHandler<UpdatePlace, Place>
    {
        private readonly IPlaceRepository _placeRepository;

        public UpdatePlaceHandler(IPlaceRepository placeRepository)
        {
            _placeRepository = placeRepository;
        }

        public async Task<Place> HandleAsync(UpdatePlace command)
        {
            var place = await _placeRepository.DemandPlaceAsync(command.Id);

            place.Name = command.Name;

            await _placeRepository.SavePlaceAsync(place);

            return place;
        }
    }
}
