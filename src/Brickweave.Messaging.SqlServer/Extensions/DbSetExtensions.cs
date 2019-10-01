using System;
using System.Collections.Generic;
using System.Linq;
using Brickweave.Messaging.Serialization;
using Brickweave.Messaging.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.Messaging.SqlServer.Extensions
{
    public static class DbSetExtensions
    {
        public static void Enqueue<TMessageData>(this DbSet<TMessageData> dbSet, IDomainEvent @event)
            where TMessageData : MessageData, new()
        {
            Enqueue(dbSet, new JsonMessageSerializer(), new[] { @event });
        }

        public static void Enqueue<TMessageData>(this DbSet<TMessageData> dbSet, IMessageSerializer serializer, IDomainEvent @event)
            where TMessageData : MessageData, new()
        {
            Enqueue(dbSet, serializer, new[] { @event });
        }

        public static void Enqueue<TMessageData>(this DbSet<TMessageData> dbSet, IEnumerable<IDomainEvent> events)
            where TMessageData : MessageData, new()
        {
            Enqueue(dbSet, new JsonMessageSerializer(), events.ToArray());
        }

        public static void Enqueue<TMessageData>(this DbSet<TMessageData> dbSet, IMessageSerializer serializer, IEnumerable<IDomainEvent> events)
            where TMessageData : MessageData, new()
        {
            Enqueue(dbSet, serializer, events.ToArray());
        }

        public static void Enqueue<TMessageData>(this DbSet<TMessageData> dbSet, params IDomainEvent[] events)
            where TMessageData : MessageData, new()
        {
            Enqueue(dbSet, new JsonMessageSerializer(), events);
        }

        public static void Enqueue<TMessageData>(this DbSet<TMessageData> dbSet, IMessageSerializer serializer, params IDomainEvent[] events)
            where TMessageData : MessageData, new()
        {
            var data = events.Select((e, i) => new TMessageData
            {
                Id = Guid.NewGuid(),
                Message = serializer.SerializeObject(e),
                Created = DateTime.Now,
                CommitSequence = i
            });

            dbSet.AddRange(data);
        }
    }
}
