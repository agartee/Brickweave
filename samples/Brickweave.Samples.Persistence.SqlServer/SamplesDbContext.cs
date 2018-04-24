using Brickweave.EventStore.SqlServer;
using Brickweave.EventStore.SqlServer.Entities;
using Brickweave.Samples.Persistence.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.Samples.Persistence.SqlServer
{
    public class SamplesDbContext : DbContext, IEventStore
    {
        public SamplesDbContext(DbContextOptions<SamplesDbContext> options) : base(options)
        {
        }
        
        public DbSet<EventData> Events { get; set; }
        public DbSet<PersonSnapshot> Persons { get; set; }
    }
}
