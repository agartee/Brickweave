using Brickweave.Cqrs.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.Cqrs.SqlServer.Tests.Data
{
    public class CqrsDbContext : DbContext
    {
        public const string SCHEMA_NAME = "Test";

        public CqrsDbContext(DbContextOptions<CqrsDbContext> options)
            : base(options)
        {

        }

        public DbSet<CommandQueueData> CommandQueue { get; set; }
        public DbSet<CommandStatusData> CommandStatus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(SCHEMA_NAME);

            base.OnModelCreating(modelBuilder);
        }
    }
}
