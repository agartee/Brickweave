using Brickweave.Domain;

namespace EventSourcingDemo.Domain.Accounts.Models
{
    public class TransactionNoteId : Id<Guid>
    {
        public TransactionNoteId(Guid value) : base(value)
        {
        }

        public static TransactionNoteId NewId()
        {
            return new TransactionNoteId(Guid.NewGuid());
        }
    }
}
