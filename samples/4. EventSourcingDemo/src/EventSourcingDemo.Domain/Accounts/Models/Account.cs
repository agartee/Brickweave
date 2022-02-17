using System.Collections.Immutable;
using Brickweave.EventStore;
using EventSourcingDemo.Domain.Accounts.Events;
using EventSourcingDemo.Domain.Common.Models;
using EventSourcingDemo.Domain.Companies.Models;
using EventSourcingDemo.Domain.People.Models;

namespace EventSourcingDemo.Domain.Accounts.Models
{
    public class Account : EventSourcedAggregateRoot
    {
        private readonly List<Transaction> _transactionHistory = new List<Transaction>();

        protected Account()
        {
            Register<PersonalAccountCreated>(Apply);
            Register<BusinessAccountCreated>(Apply);
            Register<MoneyDeposited>(Apply);
            Register<MoneyWithdrawn>(Apply);
            Register<TransactionNoteCreated>(Apply);
            Register<TransactionNoteDeleted>(Apply);
            Register<AccountDeleted>(Apply);
        }

        public Account(AccountId id, Name name, CompanyId accountHolderId) : this()
        {
            RaiseEvent(new BusinessAccountCreated(id, name, accountHolderId));
        }

        public Account(AccountId id, Name name, PersonId accountHolderId) : this()
        {
            RaiseEvent(new PersonalAccountCreated(id, name, accountHolderId));
        }

        public Account(IEnumerable<IEvent> events) : this()
        {
            ApplyEvents(events);
        }

        public AccountId Id { get; private set; }
        public LegalEntityId AccountHolderId { get; private set; }
        public Name Name { get; private set; }
        public decimal Balance { get; private set; }
        public IEnumerable<Transaction> TransactionHistory => _transactionHistory.ToImmutableList();
        public bool IsActive { get; private set; }

        public TransactionId MakeDeposit(decimal amount)
        {
            if(amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            var transactionId = TransactionId.NewId();
            var previousBalance = Balance;
            
            RaiseEvent(new MoneyDeposited(transactionId, amount, DateTime.UtcNow));
            RaiseEvent(new AccountBalanceChanged(Id, previousBalance, Balance));

            return transactionId;
        }

        public TransactionId MakeWithdrawal(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            var transactionId = TransactionId.NewId();
            var previousBalance = Balance;

            RaiseEvent(new MoneyWithdrawn(transactionId, amount, DateTime.UtcNow));
            RaiseEvent(new AccountBalanceChanged(Id, previousBalance, Balance));

            return transactionId;
        }

        public TransactionNoteId AddTransactionNote(TransactionId transactionId, string text)
        {
            var noteId = TransactionNoteId.NewId();
            RaiseEvent(new TransactionNoteCreated(transactionId, noteId, text, DateTime.UtcNow));

            return noteId;
        }

        private void Apply(PersonalAccountCreated @event)
        {
            Id = @event.AccountId;
            Name = @event.AccountName;
            AccountHolderId = @event.AccountHolderId;
        }

        private void Apply(BusinessAccountCreated @event)
        {
            Id = @event.AccountId;
            Name = @event.AccountName;
            AccountHolderId = @event.AccountHolderId;
        }

        private void Apply(MoneyDeposited @event)
        {
            _transactionHistory.Add(new Transaction(@event.TransactionId, @event.Amount, @event.Timestamp));

            Balance += @event.Amount;
        }

        private void Apply(MoneyWithdrawn @event)
        {
            _transactionHistory.Add(new Transaction(@event.TransactionId, -@event.Amount, @event.Timestamp));

            Balance -= @event.Amount;
        }

        private void Apply(TransactionNoteCreated @event)
        {
            var transaction = _transactionHistory
                .First(t => t.Id.Equals(@event.TransactionId));

            transaction.AddNote(new TransactionNote(@event.NoteId, @event.Text, @event.Created,
                EventQueue, DomainEventQueue, EventRouter));
        }

        private void Apply(TransactionNoteDeleted @event)
        {
            var result = _transactionHistory
                .SelectMany(t => t.Notes, (t, n) => new { Transaction = t, NoteId = n.Id })
                .First(o => o.NoteId.Equals(@event.NoteId));

            result.Transaction.RemoveNote(result.NoteId);
        }

        private void Apply(AccountDeleted @event)
        {
            IsActive = false;
        }
    }
}
