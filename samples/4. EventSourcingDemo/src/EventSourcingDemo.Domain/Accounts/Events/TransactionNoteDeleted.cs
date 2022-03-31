using Brickweave.EventStore;
using EventSourcingDemo.Domain.Accounts.Models;

namespace EventSourcingDemo.Domain.Accounts.Events
{
    public class TransactionNoteDeleted : IEvent
    {
        public TransactionNoteDeleted(TransactionNoteId noteId)
        {
            NoteId = noteId ?? throw new ArgumentNullException(nameof(noteId));
        }

        public TransactionNoteId NoteId { get; set; }
    }
}
