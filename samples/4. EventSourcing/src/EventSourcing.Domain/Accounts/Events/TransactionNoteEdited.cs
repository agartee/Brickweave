using Brickweave.EventStore;
using EventSourcing.Domain.Accounts.Models;

namespace EventSourcing.Domain.Accounts.Events
{
    public class TransactionNoteEdited : IChildEvent
    {
        public TransactionNoteEdited(TransactionNoteId noteId, string text, DateTime edited)
        {
            NoteId = noteId ?? throw new ArgumentNullException(nameof(noteId));
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Edited = edited;
        }

        public TransactionNoteId NoteId { get; set; }
        public string Text { get; }
        public DateTime Edited { get; }

        public object GetEntityId()
        {
            return NoteId;
        }
    }
}
