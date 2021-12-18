using AdvancedCqrs.Domain.Things.Models;
using Brickweave.Cqrs;

namespace AdvancedCqrs.Domain.Things.Commands
{
    public class UpdateThing : ICommand<Thing>
    {
        public UpdateThing(ThingId id, string name)
        {
            Id = id;
            Name = name;
        }

        public ThingId Id { get; }
        public string Name { get; }
    }
}
