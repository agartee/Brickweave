namespace EventSourcing.Domain.Common.Models
{
    public abstract class LegalEntity
    {
        public LegalEntity(LegalEntityId id, Name name)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public LegalEntityId Id { get; set; }
        public Name Name { get; set; }
    }
}
