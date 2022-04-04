using Brickweave.EventStore.SqlServer.Entities;
using Brickweave.Messaging.SqlServer.Entities;
using EventSourcing.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventSourcing.SqlServer
{
    public class EventSourcingDbContext : DbContext
    {
        public const string SCHEMA_NAME = "EventSourcing";

        public EventSourcingDbContext(DbContextOptions<EventSourcingDbContext> options) : base(options)
        {

        }

        public DbSet<IdeaData> Ideas { get; set; }
        public DbSet<EventData> Events { get; set; }
        public DbSet<MessageData> MessageOutbox { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema(SCHEMA_NAME);
        }
    }
}
