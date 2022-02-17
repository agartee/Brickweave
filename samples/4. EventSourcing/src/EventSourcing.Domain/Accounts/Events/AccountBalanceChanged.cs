using Brickweave.Domain;
using EventSourcing.Domain.Accounts.Models;

namespace EventSourcing.Domain.Accounts.Events
{
    public class AccountBalanceChanged : IDomainEvent
    {
        public AccountBalanceChanged(AccountId accountId, decimal previousBalance, decimal newBalance)
        {
            AccountId = accountId ?? throw new ArgumentNullException(nameof(accountId));
            PreviousBalance = previousBalance;
            NewBalance = newBalance;
        }

        public AccountId AccountId { get; }
        public decimal PreviousBalance { get; }
        public decimal NewBalance { get; }
    }
}
