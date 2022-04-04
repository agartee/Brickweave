using Brickweave.EventStore;
using EventSourcing.Domain.Common.Models;

namespace EventSourcing.Domain.Ideas.Events
{
    public class IdeaRenamed : IEvent
    {
        public IdeaRenamed(Name name)
        {
            Name = name;
        }

        public Name Name { get; }
    }
}
