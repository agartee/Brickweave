using System.Collections.Immutable;

namespace EventSourcing.Domain.Accounts.Models
{
    public class Transaction
    {
        private readonly List<TransactionNote> _notes = new List<TransactionNote>();

        public Transaction(TransactionId id, decimal amount, DateTime dateTime)
        {
            Id = id;
            Amount = amount;
            DateTime = dateTime;
        }

        public TransactionId Id { get; }
        public decimal Amount { get; }
        public DateTime DateTime { get; }
        public IEnumerable<TransactionNote> Notes => _notes.ToImmutableList();

        public void AddNote(TransactionNote note)
        {
            _notes.Add(note);
        }

        public void RemoveNote(TransactionNoteId noteId)
        {
            _notes.RemoveAll(n => n.Id.Equals(noteId));
        }
    }
}
