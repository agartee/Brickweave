using Brickweave.EventStore.SqlServer.Entities;
using Brickweave.Messaging.SqlServer.Entities;
using Brickweave.Samples.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.Samples.SqlServer
{
    public class SamplesDbContext : DbContext
    {
        public SamplesDbContext(DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<EventData> Events { get; set; }
        public DbSet<PersonSnapshot> Persons { get; set; }
        public DbSet<MessageData> MessageOutbox { get; set; }
    }
}
