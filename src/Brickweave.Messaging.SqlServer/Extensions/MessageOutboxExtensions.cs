using System;
using System.Collections.Generic;
using System.Linq;
using Brickweave.Messaging.Serialization;
using Brickweave.Messaging.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.Messaging.SqlServer.Extensions
{
    public static class MessageOutboxExtensions
    {
        private static IMessageSerializer DefaultSerializer => new JsonMessageSerializer();

        public static void SendDomainMessage<TMessageData>(this DbSet<TMessageData> messageDbSet, IDomainEvent @event)
            where TMessageData : MessageData, new()
        {
            SendDomainMessages(messageDbSet, DefaultSerializer, new List<IDomainEvent> { @event });
        }

        public static void SendDomainMessage<TMessageData>(this DbSet<TMessageData> messageDbSet, IMessageSerializer serializer, IDomainEvent @event)
            where TMessageData : MessageData, new()
        {
            SendDomainMessages(messageDbSet, serializer, new List <IDomainEvent> { @event });
        }

        public static void SendDomainMessages<TMessageData>(this DbSet<TMessageData> messageDbSet, params IDomainEvent[] events)
            where TMessageData : MessageData, new()
        {
            SendDomainMessages(messageDbSet, DefaultSerializer, events.ToList());
        }

        public static void SendDomainMessages<TMessageData>(this DbSet<TMessageData> messageDbSet, IMessageSerializer serializer, params IDomainEvent[] events)
            where TMessageData : MessageData, new()
        {
            SendDomainMessages(messageDbSet, serializer, events.ToList());
        }

        public static void SendDomainMessages<TMessageData>(this DbSet<TMessageData> messageDbSet, IEnumerable<IDomainEvent> events)
            where TMessageData : MessageData, new()
        {
            SendDomainMessages(messageDbSet, DefaultSerializer, events);
        }
        
        public static void SendDomainMessages<TMessageData>(this DbSet<TMessageData> messageDbSet, IMessageSerializer serializer, IEnumerable<IDomainEvent> events)
            where TMessageData : MessageData, new()
        {
            var data = events.Select((e, i) => new TMessageData
            {
                Id = Guid.NewGuid(),
                Message = serializer.SerializeObject(e),
                Created = DateTime.Now,
                CommitSequence = i
            });

            messageDbSet.AddRange(data);
        }
    }
}
