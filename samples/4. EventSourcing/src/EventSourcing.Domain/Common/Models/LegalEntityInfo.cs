namespace EventSourcing.Domain.Common.Models
{
    public class LegalEntityInfo
    {
        public LegalEntityInfo(LegalEntityId id, Name name)
        {
            Id = id;
            Name = name;
        }

        public LegalEntityId Id { get; }
        public Name Name { get; }
    }
}
