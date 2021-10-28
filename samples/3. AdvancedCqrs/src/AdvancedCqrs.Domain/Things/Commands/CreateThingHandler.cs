using System.Threading.Tasks;
using AdvancedCqrs.Domain.Things.Models;
using Brickweave.Cqrs;

namespace AdvancedCqrs.Domain.Things.Commands
{
    public class CreateThingHandler : ICommandHandler<CreateThing, Thing>
    {


        public async Task<Thing> HandleAsync(CreateThing command)
        {
            return new Thing(
                ThingId.NewId(),
                command.Name);
        }
    }
}
