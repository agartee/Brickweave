using AdvancedCqrs.SqlServer.Entities;
using Brickweave.Cqrs.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdvancedCqrs.SqlServer
{
    public class AdvancedCqrsDbContext : DbContext
    {
        public const string SCHEMA_NAME = "AdvancedCqrs";

        public AdvancedCqrsDbContext(DbContextOptions<AdvancedCqrsDbContext> options) : base(options)
        {

        }

        public DbSet<ThingData> Things { get; set; }

        public DbSet<CommandQueueData> CommandQueue { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema(SCHEMA_NAME);
        }
    }
}
