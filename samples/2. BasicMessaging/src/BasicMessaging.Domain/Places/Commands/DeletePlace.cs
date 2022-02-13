using BasicMessaging.Domain.Places.Models;
using Brickweave.Cqrs;

namespace BasicMessaging.Domain.Places.Commands
{
    public class DeletePlace : ICommand
    {
        public DeletePlace(PlaceId id)
        {
            Id = id;
        }

        public PlaceId Id { get; }
    }
}
