using System.Threading.Tasks;
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
    }
}
