using Brickweave.Domain;
using System.Collections.Generic;

namespace Brickweave.EventStore.Tests.Models
{
    public class ChildEntity : EventSourcedEntity
    {
        public ChildEntity(int id, Queue<IEvent> eventQueue, Queue<IDomainEvent> domainEventQueue, IEventRouter router) 
            : base(eventQueue, domainEventQueue, router)
        {
            Id = id;

            Register<ChildCommentSet>(Apply, Id);
        }

        public int Id { get; private set; }
        public string Comment { get; private set; }

        public void SetComment(string comment)
        {
            RaiseEvent(new ChildCommentSet(Id, comment));
        }

        private void Apply(ChildCommentSet @event)
        {
            Comment = @event.Comment;
        }
    }
}
