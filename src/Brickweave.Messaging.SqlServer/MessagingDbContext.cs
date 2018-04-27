using Brickweave.Messaging.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.Messaging.SqlServer
{
    public class MessagingDbContext : DbContext, IMessageStore
    {
        public const string SCHEMA_NAME = "Messaging";

        public MessagingDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<MessageFailureData> MessageFailures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(SCHEMA_NAME);

            base.OnModelCreating(modelBuilder);
        }
    }
}
