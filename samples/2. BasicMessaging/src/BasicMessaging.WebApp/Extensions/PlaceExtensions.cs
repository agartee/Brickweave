using BasicMessaging.Domain.Places.Models;
using BasicMessaging.WebApp.Models;

namespace BasicMessaging.WebApp.Extensions
{
    public static class PlaceExtensions
    {
        public static PlaceViewModel ToViewModel(this Place place)
        {
            return new PlaceViewModel
            {
                Id = place.Id.Value,
                Name = place.Name
            };
        }
    }
}
