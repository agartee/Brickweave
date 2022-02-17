using EventSourcingDemo.Domain.Common.Models;

namespace EventSourcingDemo.Domain.Accounts.Models
{
    public class AccountInfo
    {
        public AccountInfo(AccountId id, LegalEntityInfo accountHolder, Name name, decimal balance)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            AccountHolder = accountHolder ?? throw new ArgumentNullException(nameof(accountHolder));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Balance = balance;
        }

        public AccountId Id { get; }
        public LegalEntityInfo AccountHolder { get; }
        public Name Name { get; }
        public decimal Balance { get; }
    }
}
