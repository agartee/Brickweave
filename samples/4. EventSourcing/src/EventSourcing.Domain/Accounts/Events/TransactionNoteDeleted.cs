using Brickweave.EventStore;
using EventSourcing.Domain.Accounts.Models;

namespace EventSourcing.Domain.Accounts.Events
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
