using System.Threading.Tasks;
using BasicMessaging.Domain.Places.Services;
using Brickweave.Cqrs;

namespace BasicMessaging.Domain.Places.Commands
{
    public class DeletePlaceHandler : ICommandHandler<DeletePlace>
    {
        private readonly IPlaceRepository _placeRepository;

        public DeletePlaceHandler(IPlaceRepository PlaceRepository)
        {
            this._placeRepository = PlaceRepository;
        }

        public async Task HandleAsync(DeletePlace command)
        {
            await _placeRepository.DeletePlace(command.Id);
        }
    }
}
