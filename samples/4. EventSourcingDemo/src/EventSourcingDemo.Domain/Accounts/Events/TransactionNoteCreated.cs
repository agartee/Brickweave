using Brickweave.EventStore;
using EventSourcingDemo.Domain.Accounts.Models;

namespace EventSourcingDemo.Domain.Accounts.Events
{
    public class TransactionNoteCreated : IEvent
    {
        public TransactionNoteCreated(TransactionId transactionId, TransactionNoteId noteId, string text, DateTime created)
        {
            TransactionId = transactionId ?? throw new ArgumentNullException(nameof(transactionId));
            NoteId = noteId ?? throw new ArgumentNullException(nameof(noteId));
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Created = created;
        }

        public TransactionId TransactionId { get; }
        public TransactionNoteId NoteId { get; set; }
        public string Text { get; }
        public DateTime Created { get; }
    }
}
