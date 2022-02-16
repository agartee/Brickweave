using Brickweave.EventStore;
using EventSourcing.Domain.Accounts.Events;
using EventSourcing.Domain.Common.Models;
using EventSourcing.Domain.Companies.Models;
using EventSourcing.Domain.People.Models;

namespace EventSourcing.Domain.Accounts.Models
{
    public class Account : EventSourcedAggregateRoot
    {
        protected Account()
        {
            Register<AccountCreated>(Apply);
            Register<AccountHoldershipAssignedToCompany>(Apply);
            Register<AccountHoldershipAssignedToPerson>(Apply);
            Register<MoneyDeposited>(Apply);
            Register<MoneyWithdrawn>(Apply);
            Register<TransactionNoteCreated>(Apply);
            Register<TransactionNoteEdited>(Apply);
        }

        public Account(AccountId id, Name name, CompanyId accountHolderId) : this()
        {
            RaiseEvent(new AccountCreated(
                id ?? throw new ArgumentNullException(nameof(id)),
                name ?? throw new ArgumentNullException(nameof(name))));

            RaiseEvent(new AccountHoldershipAssignedToCompany(
                accountHolderId ?? throw new ArgumentNullException(nameof(accountHolderId))));
        }

        public Account(AccountId id, Name name, PersonId accountHolderId) : this()
        {
            RaiseEvent(new AccountCreated(
                id ?? throw new ArgumentNullException(nameof(id)),
                name ?? throw new ArgumentNullException(nameof(name))));

            RaiseEvent(new AccountHoldershipAssignedToPerson(
                accountHolderId ?? throw new ArgumentNullException(nameof(accountHolderId))));
        }

        public Account(IEnumerable<IEvent> events) : this()
        {
            ApplyEvents(events);
        }

        public AccountId Id { get; private set; }
        public LegalEntityId AccountHolderId { get; private set; }
        public Name Name { get; private set; }
        public decimal Balance { get; private set; }

        public TransactionId MakeDeposit(decimal amount)
        {
            if(amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            var transactionId = TransactionId.NewId();
            RaiseEvent(new MoneyDeposited(transactionId, amount, DateTime.UtcNow));

            return transactionId;
        }

        public TransactionId MakeWithdrawal(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount));

            var transactionId = TransactionId.NewId();
            RaiseEvent(new MoneyWithdrawn(transactionId, amount, DateTime.UtcNow));

            return transactionId;
        }

        private void Apply(AccountCreated @event)
        {
            Id = @event.AccountId;
            Name = @event.Name;
        }

        private void Apply(AccountHoldershipAssignedToCompany @event)
        {
            AccountHolderId = @event.CompanyId;
        }

        private void Apply(AccountHoldershipAssignedToPerson @event)
        {
            AccountHolderId = @event.PersonId;
        }

        private void Apply(MoneyDeposited @event)
        {
            Balance += @event.Amount;
        }

        private void Apply(MoneyWithdrawn @event)
        {
            Balance -= @event.Amount;
        }

        private void Apply(TransactionNoteCreated @event)
        {
            throw new NotImplementedException();
        }

        private void Apply(TransactionNoteEdited @event)
        {
            throw new NotImplementedException();
        }
    }
}
