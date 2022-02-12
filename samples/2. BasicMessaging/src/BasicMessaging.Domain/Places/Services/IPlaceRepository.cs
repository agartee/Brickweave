﻿using System.Collections.Generic;
using System.Threading.Tasks;
using BasicMessaging.Domain.Places.Models;

namespace BasicMessaging.Domain.Places.Services
{
    public interface IPlaceRepository
    {
        Task SavePlaceAsync(Place place);
        Task<IEnumerable<Place>> ListPlacesAsync();
    }
}
