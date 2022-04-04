using Brickweave.Domain;
using Brickweave.EventStore;
using EventSourcing.Domain.Common.Models;
using EventSourcing.Domain.Ideas.Models;

namespace EventSourcing.Domain.Ideas.Events
{
    public class IdeaCreated : IEvent, IDomainEvent
    {
        public IdeaCreated(IdeaId id, Name name)
        {
            Id = id;
            Name = name;
        }

        public IdeaId Id { get; }
        public Name Name { get; }
    }
}
