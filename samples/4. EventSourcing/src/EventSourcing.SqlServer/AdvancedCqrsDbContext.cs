using Brickweave.EventStore.SqlServer.Entities;
using EventSourcing.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdvancedCqrs.SqlServer
{
    public class EventSourcingDbContext : DbContext
    {
        public const string SCHEMA_NAME = "EventSourcing";

        public EventSourcingDbContext(DbContextOptions<EventSourcingDbContext> options) : base(options)
        {

        }

        public DbSet<EventData> Events { get; set; }
        public DbSet<PersonData> People { get; set; }
        public DbSet<CompanyData> Companies { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema(SCHEMA_NAME);
        }
    }
}
