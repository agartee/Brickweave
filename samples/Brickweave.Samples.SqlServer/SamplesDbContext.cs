using Brickweave.EventStore.SqlServer;
using Brickweave.EventStore.SqlServer.Entities;
using Brickweave.Messaging.SqlServer;
using Brickweave.Messaging.SqlServer.Entities;
using Brickweave.Samples.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.Samples.SqlServer
{
    public class SamplesDbContext : DbContext, IEventStore, IMessageStore
    {
        public SamplesDbContext(DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<EventData> Events { get; set; }
        public DbSet<MessageFailureData> MessageFailures { get; set; }
        public DbSet<PersonSnapshot> Persons { get; set; }
    }
}
