using System;
using System.Collections.Generic;
using System.Linq;
using Brickweave.Domain;
using Brickweave.Serialization;
using Brickweave.Messaging.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.Messaging.SqlServer.Extensions
{
    public static class DbSetExtensions
    {
        public static void EnqueueDomainEvents<TMessageData>(this DbSet<TMessageData> dbSet, IDocumentSerializer serializer, IDomainEvent @event)
            where TMessageData : MessageData, new()
        {
            EnqueueDomainEvents(dbSet, serializer, new[] { @event });
        }

        public static void EnqueueDomainEvents<TMessageData>(this DbSet<TMessageData> dbSet, IDocumentSerializer serializer, IEnumerable<IDomainEvent> events)
            where TMessageData : MessageData, new()
        {
            EnqueueDomainEvents(dbSet, serializer, events.ToArray());
        }

        public static void EnqueueDomainEvents<TMessageData>(this DbSet<TMessageData> dbSet, IDocumentSerializer serializer, params IDomainEvent[] events)
            where TMessageData : MessageData, new()
        {
            var data = events.Select((e, i) => new TMessageData
            {
                Id = Guid.NewGuid(),
                TypeName = e.GetType().Name,
                Json = serializer.SerializeObject(e),
                Created = DateTime.UtcNow,
                CommitSequence = i
            });

            dbSet.AddRange(data);
        }
    }
}
