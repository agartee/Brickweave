using BasicMessaging.SqlServer.Entities;
using Brickweave.Messaging.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace BasicMessaging.SqlServer
{
    public class MessagingDemoDbContext : DbContext
    {
        public MessagingDemoDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<PlaceData> Places { get; set; }
        public DbSet<MessageData> MessageOutbox { get; set; }
    }
}
