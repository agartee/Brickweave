using BasicMessaging.Domain.Places.Models;
using Brickweave.Cqrs;

namespace BasicMessaging.Domain.Places.Commands
{
    public class UpdatePlace : ICommand<Place>
    {
        public UpdatePlace(PlaceId id, string name)
        {
            Id = id;
            Name = name;
        }

        public PlaceId Id { get; }
        public string Name { get; }
    }
}
