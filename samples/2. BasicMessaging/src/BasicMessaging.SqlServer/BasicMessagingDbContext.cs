using BasicMessaging.SqlServer.Entities;
using Brickweave.Messaging.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace BasicMessaging.SqlServer
{
    public class BasicMessagingDbContext : DbContext
    {
        public BasicMessagingDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<PlaceData> Places { get; set; }
        public DbSet<MessageData> MessageOutbox { get; set; }
    }
}
