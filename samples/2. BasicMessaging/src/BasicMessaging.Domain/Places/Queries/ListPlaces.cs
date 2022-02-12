using System.Collections.Generic;
using BasicMessaging.Domain.Places.Models;
using Brickweave.Cqrs;

namespace BasicMessaging.Domain.Places.Queries
{
    /// <summary>
    /// Gets a list of existing places.
    /// </summary>
    public class ListPlaces : IQuery<IEnumerable<Place>>
    {
    }
}
