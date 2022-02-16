using EventSourcing.Domain.Accounts.Models;

namespace EventSourcing.Domain.Accounts.Events
{
    public class TransactionNoteEdited
    {
        public TransactionNoteEdited(NoteId noteId, string text)
        {
            NoteId = noteId;
            Text = text;
        }

        public NoteId NoteId { get; set; }
        public string Text { get; }
    }
}
