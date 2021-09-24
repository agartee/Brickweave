using System;
using BasicMessaging.Domain.Places.Models;
using Brickweave.Domain;

namespace BasicMessaging.Domain.Places.Events
{
    public class PlaceCreated : IDomainEvent
    {
        public PlaceCreated(PlaceId id, string name)
        {
            Id = id;
            Name = name;
        }

        public PlaceId Id { get; }
        public string Name { get; }
    }
}
