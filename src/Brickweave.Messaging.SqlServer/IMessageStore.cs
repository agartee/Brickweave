using Brickweave.Messaging.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.Messaging.SqlServer
{
    public interface IMessageStore
    {
        DbSet<MessageFailureData> MessageFailures { get; set; }
    }
}
