using System.Collections.Generic;
using BasicMessaging.Domain.Places.Events;
using Brickweave.Domain;
using LiteGuard;

namespace BasicMessaging.Domain.Places.Models
{
    public class Place : DomainModel
    {
        public Place(PlaceId id, string name) : base(new Queue<IDomainEvent>())
        {
            Guard.AgainstNullArgument(nameof(id), id);
            Guard.AgainstNullArgument(nameof(name), name);

            Id = id;
            Name = name;

            RaiseEvent(new PlaceCreated(id, name));
        }

        public PlaceId Id { get; }
        public string Name { get; }
    }
}
