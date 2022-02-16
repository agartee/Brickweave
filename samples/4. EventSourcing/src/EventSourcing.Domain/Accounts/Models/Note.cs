using Brickweave.Domain;
using Brickweave.EventStore;

namespace EventSourcing.Domain.Accounts.Models
{
    public class Note : EventSourcedEntity
    {
        public Note(string text, DateTime created, Queue<IEvent> eventQueue, Queue<IDomainEvent> domainEventQueue, IEventRouter eventRouter)
            : base(eventQueue, domainEventQueue, eventRouter)
        {
            Text = text;
            Created = created;
        }

        public string Text { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime LastUpdated { get; private set; }

        public void EditText(string text)
        {

        }
    }
}
