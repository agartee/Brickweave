using System;
using System.Linq;
using System.Threading.Tasks;
using Brickweave.EventStore.Factories;
using Brickweave.EventStore.Serialization;
using Brickweave.EventStore.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.EventStore.SqlServer
{
    public abstract class AggregateRepository<TAggregate> where TAggregate : EventSourcedAggregateRoot
    {
        private readonly IDocumentSerializer _serializer;
        private readonly IAggregateFactory _aggregateFactory;

        protected AggregateRepository(IDocumentSerializer serializer, IAggregateFactory aggregateFactory)
        {
            _serializer = serializer;
            _aggregateFactory = aggregateFactory;
        }

        protected void AddUncommittedEvents<TEventData>(DbSet<TEventData> eventDbSet, 
            EventSourcedAggregateRoot aggregate, Guid streamId) 
            where TEventData : EventData, new()
        {
            var created = DateTime.UtcNow;

            var uncommittedEvents = aggregate.GetUncommittedEvents()
                .Select((e, i) => new TEventData
                {
                    Id = Guid.NewGuid(),
                    StreamId = streamId,
                    Json = _serializer.SerializeObject(e),
                    Created = created,
                    CommitSequence = i
                })
                .ToList();

            uncommittedEvents.ForEach(e => eventDbSet.Add(e));
        }

        protected async Task<TAggregate> CreateFromEventsAsync<TEventData>(DbSet<TEventData> eventDbSet,
            Guid streamId)
            where TEventData : EventData, new()
        {
            var eventData = await eventDbSet
                .Where(e => e.StreamId.Equals(streamId))
                .OrderBy(e => e.Created)
                .ThenBy(e => e.CommitSequence)
                .ToListAsync();

            var events = eventData
                .Select(d => _serializer.DeserializeObject<IEvent>(d.Json))
                .ToList();

            return events.Any() ? _aggregateFactory.Create<TAggregate>(events) : null;
        }
    }
}
