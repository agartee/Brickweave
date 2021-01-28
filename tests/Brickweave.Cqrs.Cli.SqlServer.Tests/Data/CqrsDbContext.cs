using Brickweave.Cqrs.Cli.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.Cqrs.Cli.SqlServer.Tests.Data
{
    public class CqrsDbContext : DbContext
    {
        public const string SCHEMA_NAME = "Cqrs";

        public CqrsDbContext(DbContextOptions<CqrsDbContext> options)
            : base(options)
        {

        }

        public DbSet<ExecutionStatusData> ExecutionStatus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(SCHEMA_NAME);

            base.OnModelCreating(modelBuilder);
        }
    }
}
