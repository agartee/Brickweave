using System.Collections.Generic;
using System.Linq;
using Brickweave.Domain;
using Brickweave.Serialization;
using Brickweave.Messaging.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.Messaging.SqlServer.Extensions
{
    public static class DomainEventExtensions
    {
        public static void Enqueue<TMessageData>(this IEnumerable<IDomainEvent> events, DbSet<TMessageData> dbSet, IDocumentSerializer serializer)
            where TMessageData : MessageData, new()
        {
            dbSet.EnqueueDomainEvents(serializer, events.ToArray());
        }
    }
}
