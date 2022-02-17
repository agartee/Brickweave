using System.Collections.Immutable;

namespace EventSourcing.Domain.Accounts.Models
{
    public class Transaction
    {
        private readonly List<Note> _notes = new List<Note>();

        public Transaction(TransactionId id, decimal amount, DateTime dateTime)
        {
            Id = id;
            Amount = amount;
            DateTime = dateTime;
        }

        public TransactionId Id { get; }
        public decimal Amount { get; }
        public DateTime DateTime { get; }
        public IEnumerable<Note> Notes => _notes.ToImmutableList();

        public void AddNote(Note note)
        {
            _notes.Add(note);
        }

        public void RemoveNote(NoteId noteId)
        {
            _notes.RemoveAll(n => n.Id.Equals(noteId));
        }
    }
}
