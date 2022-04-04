using System;
using Brickweave.EventStore;
using EventSourcing.Domain.Common.Models;
using EventSourcing.Domain.Ideas.Events;

namespace EventSourcing.Domain.Ideas.Models
{
    public class Idea : EventSourcedAggregateRoot
    {
        Idea()
        {
            Register<IdeaCreated>(Apply);
            Register<IdeaRenamed>(Apply);
        }

        public Idea(IdeaId id, Name name)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));

            RaiseEvent(new IdeaCreated(id, name));
        }

        public IdeaId Id { get; private set; }
        public Name Name { get; private set; }

        public void Rename(Name newName)
        {
            if(newName == null)
                throw new ArgumentNullException(nameof(newName));

            RaiseEvent(new IdeaRenamed(newName));
        }

        private void Apply(IdeaCreated @event)
        {
            Id = @event.Id;
            Name = @event.Name;
        }

        private void Apply(IdeaRenamed @event)
        {
            Name = @event.Name;
        }
    }
}
