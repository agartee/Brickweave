using System;
using System.Collections.Generic;
using BasicMessaging.Domain.Places.Events;
using Brickweave.Domain;

namespace BasicMessaging.Domain.Places.Models
{
    public class Place : DomainModel
    {
        public Place(PlaceId id, string name, bool isNew = false) : base(new Queue<IDomainEvent>())
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));

            if(isNew)
                RaiseEvent(new PlaceCreated(id, name));
        }

        public PlaceId Id { get; }
        public string Name { get; set; }
    }
}
