using BasicMessaging.Domain.Places.Models;
using Brickweave.Cqrs;

namespace BasicMessaging.Domain.Places.Queries
{
    public class GetPlace : IQuery<Place>
    {
        public GetPlace(PlaceId id)
        {
            Id = id;
        }

        public PlaceId Id { get; }
    }
}
