namespace EventSourcingDemo.Domain.Common.Models
{
    public class LegalEntityInfo
    {
        public LegalEntityInfo(LegalEntityId id, Name name)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public LegalEntityId Id { get; }
        public Name Name { get; }
    }
}
