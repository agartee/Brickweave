using Brickweave.EventStore.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.EventStore.SqlServer
{
    public class EventStoreContext : DbContext
    {
        public const string SCHEMA_NAME = "EventStore";

        public EventStoreContext(DbContextOptions options)
            : base(options)
        {
            
        }

        public DbSet<EventData> Events { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(SCHEMA_NAME);

            base.OnModelCreating(modelBuilder);
        }
    }
}
