using Brickweave.EventStore.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.EventStore.SqlServer
{
    public class EventStoreContext : DbContext
    {
        private readonly string _schema;

        public EventStoreContext(DbContextOptions options, string schema)
            : base(options)
        {
            _schema = schema;
        }

        public DbSet<EventData> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(_schema);

            base.OnModelCreating(modelBuilder);
        }
    }
}
