using System;
using System.Linq;
using System.Threading.Tasks;
using Brickweave.EventStore.Factories;
using Brickweave.EventStore.Serialization;
using Brickweave.EventStore.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.EventStore.SqlServer
{
    public abstract class SqlServerAggregateRepository<TAggregate, TContext>
        where TAggregate : EventSourcedAggregateRoot
        where TContext : DbContext, IEventStore
    {
        private readonly DbSet<EventData> _events;
        private readonly IDocumentSerializer _serializer;
        private readonly IAggregateFactory _aggregateFactory;

        protected SqlServerAggregateRepository(DbSet<EventData> events, IDocumentSerializer serializer,
            IAggregateFactory aggregateFactory)
        {
            _events = events;
            _aggregateFactory = aggregateFactory;
            _serializer = serializer;
        }
        
        protected void AddUncommittedEvents(EventSourcedAggregateRoot aggregate, Guid streamId)
        {
            var created = DateTime.UtcNow;
            
            var uncommittedEvents = aggregate.GetUncommittedEvents()
                .Select((e, i) => CreateEventData(streamId, e, created, i))
                .ToList();

            uncommittedEvents.ForEach(e => _events.Add(e));
            
            aggregate.ClearUncommittedEvents();
        }
        
        protected async Task<TAggregate> GetFromEventsAsync(Guid streamId)
        {
            var eventData = await _events
                .Where(e => e.StreamId.Equals(streamId))
                .OrderBy(e => e.Created)
                .ThenBy(e => e.CommitSequence)
                .ToListAsync();

            var events = eventData
                .Select(d => _serializer.DeserializeObject<IEvent>(d.Json))
                .ToList();

            return events.Any() ? _aggregateFactory.Create<TAggregate>(events) : null;
        }

        protected async Task RemoveEventsAsync(Guid streamId, Func<Task> onBeforeSaveChanges = null)
        {
            var eventData = await _events
                .Where(e => e.StreamId.Equals(streamId))
                .ToListAsync();

            _events.RemoveRange(eventData);
        }

        private EventData CreateEventData(Guid streamId, IEvent @event, DateTime created, int commitSequence)
        {
            return new EventData
            {
                Id = Guid.NewGuid(),
                StreamId = streamId,
                Json = _serializer.SerializeObject(@event),
                Created = created,
                CommitSequence = commitSequence
            };
        }
    }
}
