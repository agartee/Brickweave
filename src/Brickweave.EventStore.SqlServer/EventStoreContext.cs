using Brickweave.EventStore.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.EventStore.SqlServer
{
    public class EventStoreContext : DbContext
    {
        public EventStoreContext(DbContextOptions options, string schema = "EventStore")
            : base(options)
        {
            Schema = schema;
        }

        public string Schema { get; }
        public DbSet<EventData> Events { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);

            base.OnModelCreating(modelBuilder);
        }
    }
}
