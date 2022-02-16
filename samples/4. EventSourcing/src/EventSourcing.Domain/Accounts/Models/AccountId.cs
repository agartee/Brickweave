using Brickweave.Domain;

namespace EventSourcing.Domain.Accounts.Models
{
    public class AccountId : Id<Guid>
    {
        public AccountId(Guid value) : base(value)
        {
        }

        public static AccountId NewId()
        {
            return new AccountId(Guid.NewGuid());
        }
    }
}
