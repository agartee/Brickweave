using Brickweave.Domain;

namespace EventSourcing.Domain.Common.Models
{
    public abstract class LegalEntityId : Id<Guid>
    {
        public LegalEntityId(Guid value) : base(value)
        {
        }
    }
}
