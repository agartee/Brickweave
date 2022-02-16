using Brickweave.Domain;

namespace EventSourcing.Domain.Accounts.Models
{
    public class NoteId : Id<Guid>
    {
        public NoteId(Guid value) : base(value)
        {
        }

        public static NoteId NewId()
        {
            return new NoteId(Guid.NewGuid());
        }
    }
}
