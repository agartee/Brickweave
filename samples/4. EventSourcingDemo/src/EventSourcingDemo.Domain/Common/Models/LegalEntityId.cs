using Brickweave.Domain;

namespace EventSourcingDemo.Domain.Common.Models
{
    public abstract class LegalEntityId : Id<Guid>
    {
        public LegalEntityId(Guid value) : base(value)
        {
        }
    }
}
