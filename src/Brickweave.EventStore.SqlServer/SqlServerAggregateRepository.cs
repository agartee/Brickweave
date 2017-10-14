using System;
using System.Linq;
using System.Threading.Tasks;
using Brickweave.Domain;
using Brickweave.EventStore.Factories;
using Brickweave.EventStore.Serialization;
using Brickweave.EventStore.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.EventStore.SqlServer
{
    public abstract class SqlServerAggregateRepository<TAggregate, TIdentity>
        where TAggregate : EventSourcedAggregateRoot<TIdentity>
    {
        private readonly EventStoreContext _dbContext;
        private readonly IDocumentSerializer _serializer;
        private readonly IAggregateFactory _aggregateFactory;

        protected SqlServerAggregateRepository(EventStoreContext dbContext, IDocumentSerializer serializer,
            IAggregateFactory aggregateFactory)
        {
            _dbContext = dbContext;
            _aggregateFactory = aggregateFactory;
            _serializer = serializer;
        }

        protected async Task SaveUncommittedEventsAsync(EventSourcedAggregateRoot<TIdentity> aggregate, Id<Guid> id)
        {
            var created = DateTime.UtcNow;
            
            var uncommittedEvents = aggregate.GetUncommittedEvents()
                .Select((e, i) => CreateEventData(id, e, created, i))
                .ToList();

            uncommittedEvents.ForEach(e => _dbContext.Events.Add(e));
            await _dbContext.SaveChangesAsync();
            aggregate.ClearUncommittedEvents();
        }

        protected async Task<bool> ExistsAsync(Id<Guid> aggregateId)
        {
            var aggregate = await _dbContext.Events
                .FirstOrDefaultAsync(i => i.StreamId.Equals(aggregateId.Value));

            return aggregate != null;
        }

        protected async Task<TAggregate> TryFindAsync(Id<Guid> aggregateId)
        {
            if (aggregateId == null)
                return null;

            var eventData = await _dbContext.Events
                .Where(e => e.StreamId.Equals(aggregateId.Value))
                .OrderBy(e => e.Created)
                .ThenBy(e => e.CommitSequence)
                .ToListAsync();

            var events = eventData
                .Select(d => _serializer.DeserializeObject<IAggregateEvent>(d.Json))
                .ToList();

            return events.Any() ? _aggregateFactory.Create<TAggregate>(events) : null;
        }

        protected async Task DeleteAsync(Id<Guid> aggregateId)
        {
            if (aggregateId == null)
                return;

            var eventData = await _dbContext.Events
                .Where(e => e.StreamId.Equals(aggregateId.Value))
                .ToListAsync();

            _dbContext.Events.RemoveRange(eventData);

            await _dbContext.SaveChangesAsync();
        }

        private EventData CreateEventData(Id<Guid> id, IAggregateEvent @event, DateTime created, int commitSequence)
        {
            return new EventData
            {
                EventId = Guid.NewGuid(),
                StreamId = id.Value,
                Json = _serializer.SerializeObject(@event),
                Created = created,
                CommitSequence = commitSequence
            };
        }
    }
}
