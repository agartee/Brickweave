using Brickweave.EventStore;
using EventSourcing.Domain.Accounts.Models;

namespace EventSourcing.Domain.Accounts.Events
{
    public class TransactionNoteDeleted : IEvent
    {
        public TransactionNoteDeleted(NoteId noteId)
        {
            NoteId = noteId ?? throw new ArgumentNullException(nameof(noteId));
        }

        public NoteId NoteId { get; set; }
    }
}
