using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brickweave.EventStore.Factories;
using Brickweave.Serialization;
using Brickweave.EventStore.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.EventStore.SqlServer
{
    public abstract class AggregateRepository<TAggregate> where TAggregate : EventSourcedAggregateRoot
    {
        private readonly IAggregateFactory _aggregateFactory;
        private readonly IDocumentSerializer _documentSerializer;

        protected AggregateRepository(IDocumentSerializer serializer, IAggregateFactory aggregateFactory)
        {
            _documentSerializer = serializer;
            _aggregateFactory = aggregateFactory;
        }

        protected async Task<IEnumerable<IEvent>> GetEvents<TEventData>(DbSet<TEventData> eventDbSet, 
            Guid streamId, DateTime? pointInTime = null)
            where TEventData : EventData, new()
        {
            var eventData = await eventDbSet
                .Where(e => e.StreamId.Equals(streamId))
                .Where(e => pointInTime == null || e.Created <= pointInTime)
                .OrderBy(e => e.Created)
                .ThenBy(e => e.CommitSequence)
                .ToListAsync();

            var events = eventData
                .Select(d => _documentSerializer.DeserializeObject<IEvent>(d.Json))
                .ToList();

            return events;
        }

        protected async Task<IEnumerable<IGrouping<Guid, IEvent>>> GetEvents<TEventData>(DbSet<TEventData> eventDbSet,
            IEnumerable<Guid> streamIds, DateTime? pointInTime = null)
            where TEventData : EventData, new()
        {
            var eventData = await eventDbSet
                .Where(e => streamIds.Contains(e.StreamId))
                .Where(e => pointInTime == null || e.Created <= pointInTime)
                .OrderBy(e => e.Created)
                .ThenBy(e => e.CommitSequence)
                .ToListAsync();

            var events = eventData
                .GroupBy(d => d.StreamId, d => _documentSerializer.DeserializeObject<IEvent>(d.Json))
                .ToList();

            return events;
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
                    Json = _documentSerializer.SerializeObject(e),
                    Created = created,
                    CommitSequence = i
                })
                .ToList();

            uncommittedEvents.ForEach(e => eventDbSet.Add(e));
        }

        protected async Task<TAggregate> CreateFromEventsAsync<TEventData>(DbSet<TEventData> eventDbSet,
            Guid streamId, DateTime? pointInTime = null)
            where TEventData : EventData, new()
        {
            var events = await GetEvents(eventDbSet, streamId, pointInTime);

            return events.Any() ? _aggregateFactory.Create<TAggregate>(events) : null;
        }

        protected async Task<IEnumerable<TAggregate>> CreateFromEventsAsync<TEventData>(DbSet<TEventData> eventDbSet,
            IEnumerable<Guid> streamIds, DateTime? pointInTime = null)
            where TEventData : EventData, new()
        {
            var eventGroups = await GetEvents(eventDbSet, streamIds, pointInTime);

            if (!eventGroups.Any())
                return Enumerable.Empty<TAggregate>();

            return eventGroups
                .Select(g => _aggregateFactory.Create<TAggregate>(g.ToList()))
                .ToList();
        }

        protected async Task DeleteEvents<TEventData>(DbSet<TEventData> eventDbSet, Guid streamId)
            where TEventData : EventData, new()
        {
            var eventData = await eventDbSet
                .Where(e => e.StreamId.Equals(streamId))
                .ToListAsync();

            eventDbSet.RemoveRange(eventData);
        }
    }
}
