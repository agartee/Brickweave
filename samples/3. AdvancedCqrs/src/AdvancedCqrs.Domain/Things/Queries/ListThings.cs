using System.Collections.Generic;
using AdvancedCqrs.Domain.Things.Models;
using Brickweave.Cqrs;

namespace AdvancedCqrs.Domain.Things.Queries
{
    public class ListThings : IQuery<IEnumerable<Thing>>
    {
    }
}
