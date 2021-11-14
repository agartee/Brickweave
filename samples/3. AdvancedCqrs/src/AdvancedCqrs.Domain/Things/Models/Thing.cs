namespace AdvancedCqrs.Domain.Things.Models
{
    public class Thing
    {
        public Thing(ThingId id, string name)
        {
            Id = id;
            Name = name;
        }

        public ThingId Id { get; }
        public string Name { get; set; }
    }
}
