using Brickweave.EventStore.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.EventStore.SqlServer
{
    public interface IEventStore
    {
        DbSet<EventData> Events { get; set; }
    }
}