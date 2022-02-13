using System.Collections.Generic;
using System.Threading.Tasks;
using BasicMessaging.Domain.Places.Models;

namespace BasicMessaging.Domain.Places.Services
{
    public interface IPlaceRepository
    {
        Task SavePlaceAsync(Place place);
        Task<Place> DemandPlaceAsync(PlaceId id);
        Task<IEnumerable<Place>> ListPlacesAsync();
        Task DeletePlace(PlaceId id);
    }
}
