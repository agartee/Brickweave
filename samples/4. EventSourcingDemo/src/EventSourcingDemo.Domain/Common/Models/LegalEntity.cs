namespace EventSourcingDemo.Domain.Common.Models
{
    public abstract class LegalEntity<T> where T : LegalEntityId
    {
        public LegalEntity(T id, Name name)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public T Id { get; set; }
        public Name Name { get; set; }
    }
}
