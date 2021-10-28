using Brickweave.Cqrs.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdvancedCqrs.CommandQueue.SqlServer
{
    public class CommandQueueDbContext : DbContext
    {
        public const string SCHEMA_NAME = "SeparateCommandQueue";

        public CommandQueueDbContext(DbContextOptions<CommandQueueDbContext> options) : base(options)
        {

        }

        public DbSet<CommandQueueData> CommandQueue { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema(SCHEMA_NAME);
        }
    }
}
