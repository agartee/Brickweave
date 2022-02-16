using EventSourcing.Domain.Accounts.Models;

namespace EventSourcing.Domain.Accounts.Events
{
    public class TransactionNoteCreated
    {
        public TransactionNoteCreated(NoteId noteId, string text)
        {
            NoteId = noteId;
            Text = text;
        }

        public NoteId NoteId { get; set; }
        public string Text { get; }
    }
}
