using Brickweave.Domain;

namespace EventSourcingDemo.Domain.Accounts.Models
{
    public class TransactionId : Id<Guid>
    {
        public TransactionId(Guid value) : base(value)
        {
        }

        public static TransactionId NewId()
        {
            return new TransactionId(Guid.NewGuid());
        }
    }
}
