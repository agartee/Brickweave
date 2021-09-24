using System;
using Brickweave.Domain;

namespace BasicMessaging.Domain.Places.Models
{
    public class PlaceId : Id<Guid>
    {
        public PlaceId(Guid value) : base(value)
        {
        }

        public static PlaceId NewId()
        {
            return new PlaceId(Guid.NewGuid());
        }
    }
}
