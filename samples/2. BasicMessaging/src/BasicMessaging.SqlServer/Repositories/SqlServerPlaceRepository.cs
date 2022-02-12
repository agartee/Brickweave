using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasicMessaging.Domain.Places.Models;
using BasicMessaging.Domain.Places.Services;
using BasicMessaging.SqlServer.Entities;
using Brickweave.Messaging.SqlServer.Extensions;
using Brickweave.Serialization;
using Microsoft.EntityFrameworkCore;

namespace BasicMessaging.SqlServer.Repositories
{
    public class SqlServerPlaceRepository : IPlaceRepository
    {
        private readonly BasicMessagingDbContext _dbContext;
        private readonly IDocumentSerializer _serializer;

        public SqlServerPlaceRepository(BasicMessagingDbContext dbContext, IDocumentSerializer serializer)
        {
            _dbContext = dbContext;
            _serializer = serializer;
        }

        public async Task SavePlaceAsync(Place place)
        {
            var data = new PlaceData
            {
                Id = place.Id.Value,
                Name = place.Name
            };

            _dbContext.Places.Add(data);

            place.GetDomainEvents()
                .Enqueue(_dbContext.MessageOutbox, _serializer);

            await _dbContext.SaveChangesAsync();

            place.ClearDomainEvents();
        }

        public async Task<IEnumerable<Place>> ListPlacesAsync()
        {
            var data = await _dbContext.Places
                .ToListAsync();

            return data.Select(t =>
                new Place(
                    new PlaceId(t.Id),
                    t.Name))
                .ToList();
        }
    }
}
