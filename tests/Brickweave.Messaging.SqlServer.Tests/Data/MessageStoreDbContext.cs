using Brickweave.Messaging.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.Messaging.SqlServer.Tests.Data
{
    public class MessageStoreDbContext : DbContext
    {
        public const string SCHEMA_NAME = "Messaging";

        public MessageStoreDbContext(DbContextOptions<MessageStoreDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<MessageData> MessageOutbox { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(SCHEMA_NAME);

            base.OnModelCreating(modelBuilder);
        }
    }
}
