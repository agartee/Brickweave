using System.Threading.Tasks;
using AdvancedCqrs.Domain.Things.Models;
using AdvancedCqrs.Domain.Things.Services;
using Brickweave.Cqrs;

namespace AdvancedCqrs.Domain.Things.Commands
{
    public class CreateThingHandler : ICommandHandler<CreateThing, Thing>
    {
        private readonly IThingRepository _thingRepository;

        public CreateThingHandler(IThingRepository thingRepository)
        {
            _thingRepository = thingRepository;
        }

        public async Task<Thing> HandleAsync(CreateThing command)
        {
            await Task.Delay(10000); // long-running command for demo should take a bit ;)

            var thing = new Thing(ThingId.NewId(), command.Name);
            
            await _thingRepository.SaveThingAsync(thing);

            return thing;
        }
    }
}
