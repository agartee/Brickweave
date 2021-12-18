using System.Collections.Generic;
using System.Threading.Tasks;
using AdvancedCqrs.Domain.Things.Models;

namespace AdvancedCqrs.Domain.Things.Services
{
    public interface IThingRepository
    {
        Task SaveThingAsync(Thing thing);
        Task<Thing> DemandThingAsync(ThingId id);
        Task<IEnumerable<Thing>> ListThingsAsync();
    }
}
