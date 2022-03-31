using EventSourcingDemo.Domain.Common.Models;

namespace EventSourcingDemo.Domain.Accounts.Models
{
    public class AccountInfo
    {
        public AccountInfo(AccountId id, Name name, LegalEntityInfo accountHolder, decimal balance)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            AccountHolder = accountHolder ?? throw new ArgumentNullException(nameof(accountHolder));
            Balance = balance;
        }

        public AccountId Id { get; }
        public LegalEntityInfo AccountHolder { get; }
        public Name Name { get; }
        public decimal Balance { get; }
    }
}
