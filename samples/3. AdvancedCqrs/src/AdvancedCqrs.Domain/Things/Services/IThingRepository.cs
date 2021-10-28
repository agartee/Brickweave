using System.Threading.Tasks;
using AdvancedCqrs.Domain.Things.Models;

namespace AdvancedCqrs.Domain.Things.Services
{
    public interface IThingRepository
    {
        Task SaveThingAsync(Thing thing);
    }
}
