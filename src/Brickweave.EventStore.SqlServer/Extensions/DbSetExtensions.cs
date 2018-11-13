using System;
using System.Linq;
using System.Threading.Tasks;
using Brickweave.EventStore.Factories;
using Brickweave.EventStore.Serialization;
using Brickweave.EventStore.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.EventStore.SqlServer.Extensions
{
    public static class DbSetExtensions
    {
        public static void AddUncommittedEvents(this DbSet<EventData> eventDbSet, 
            EventSourcedAggregateRoot aggregate, Guid streamId, IDocumentSerializer serializer = null)
        {
            if (serializer == null)
                serializer = SystemDefaults.DocumentSerializer;

            var created = DateTime.UtcNow;

            var uncommittedEvents = aggregate.GetUncommittedEvents()
                .Select((e, i) => new EventData
                {
                    Id = Guid.NewGuid(),
                    StreamId = streamId,
                    Json = serializer.SerializeObject(e),
                    Created = created,
                    CommitSequence = i
                })
                .ToList();

            uncommittedEvents.ForEach(e => eventDbSet.Add(e));

            aggregate.ClearUncommittedEvents();
        }

        public static async Task<TAggregate> CreateFromEventsAsync<TAggregate>(this DbSet<EventData> eventDbSet, 
            Guid streamId, IDocumentSerializer serializer = null, IAggregateFactory aggregateFactory = null) 
            where TAggregate : EventSourcedAggregateRoot
        {
            if (serializer == null)
                serializer = SystemDefaults.DocumentSerializer;

            if (aggregateFactory == null)
                aggregateFactory = SystemDefaults.AggregateFactory;

            var eventData = await eventDbSet
                .Where(e => e.StreamId.Equals(streamId))
                .OrderBy(e => e.Created)
                .ThenBy(e => e.CommitSequence)
                .ToListAsync();
            
            var events = eventData
                .Select(d => serializer.DeserializeObject<IEvent>(d.Json))
                .ToList();

            return events.Any() ? aggregateFactory.Create<TAggregate>(events) : null;
        }

        public static async Task RemoveEventsAsync(this DbSet<EventData> eventDbSet, Guid streamId)
        {
            var eventData = await eventDbSet
                .Where(e => e.StreamId.Equals(streamId))
                .ToListAsync();

            eventDbSet.RemoveRange(eventData);
        }
    }
}
