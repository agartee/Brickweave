using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdvancedCqrs.Domain.Common.Exceptions;
using AdvancedCqrs.Domain.Things.Models;
using AdvancedCqrs.Domain.Things.Services;
using AdvancedCqrs.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdvancedCqrs.SqlServer.Repositories
{
    public class SqlServerThingRepository : IThingRepository
    {
        private readonly AdvancedCqrsDbContext _dbContext;

        public SqlServerThingRepository(AdvancedCqrsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveThingAsync(Thing thing)
        {
            var data = await _dbContext.Things
                .SingleOrDefaultAsync(t => t.Id == thing.Id.Value);

            if(data == null)
            {
                _dbContext.Things.Add(new ThingData
                {
                    Id = thing.Id.Value,
                    Name = thing.Name
                });
            }
            else
            {
                data.Name = thing.Name;
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<Thing> DemandThingAsync(ThingId id)
        {
            var data = await _dbContext.Things
                .SingleOrDefaultAsync(t => t.Id == id.Value);

            if(data == null)
                throw new EntityNotFoundException(id, nameof(Thing));

            return new Thing(
                new ThingId(data.Id),
                data.Name);
        }

        public async Task<IEnumerable<Thing>> ListThingsAsync()
        {
            var data = await _dbContext.Things
                .ToListAsync();

            return data.Select(t =>
                new Thing(
                    new ThingId(t.Id),
                    t.Name))
                .ToList();
        }
    }
}
