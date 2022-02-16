using EventSourcing.Domain.Common.Models;

namespace EventSourcing.Domain.Accounts.Models
{
    public class AccountInfo
    {
        public AccountInfo(AccountId id, LegalEntityInfo accountHolder, Name name)
        {
            Id = id;
            AccountHolder = accountHolder;
            Name = name;
        }

        public AccountId Id { get; }
        public LegalEntityInfo AccountHolder { get; }
        public Name Name { get; }
    }
}
