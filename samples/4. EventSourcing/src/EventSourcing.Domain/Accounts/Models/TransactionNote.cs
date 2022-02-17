using Brickweave.Domain;
using Brickweave.EventStore;
using EventSourcing.Domain.Accounts.Events;

namespace EventSourcing.Domain.Accounts.Models
{
    public class TransactionNote : EventSourcedEntity
    {
        private TransactionNote(TransactionNoteId id, Queue<IEvent> eventQueue, Queue<IDomainEvent> domainEventQueue, IEventRouter eventRouter)
            : base(eventQueue, domainEventQueue, eventRouter)
        {
            Register<TransactionNoteEdited>(Apply, id);
        }

        public TransactionNote(TransactionNoteId id, string text, DateTime created, Queue<IEvent> eventQueue, Queue<IDomainEvent> domainEventQueue, IEventRouter eventRouter)
            : this(id, eventQueue, domainEventQueue, eventRouter)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Created = created;
        }

        public TransactionNoteId Id { get; }
        public string Text { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime LastUpdated { get; private set; }

        public void EditText(string text)
        {
            RaiseEvent(new TransactionNoteEdited(Id, text, DateTime.UtcNow));
        }

        public void Delete()
        {
            RaiseEvent(new TransactionNoteDeleted(Id));
        }

        private void Apply(TransactionNoteEdited @event)
        {
            Text = @event.Text;
            LastUpdated = @event.Edited;
        }
    }
}
