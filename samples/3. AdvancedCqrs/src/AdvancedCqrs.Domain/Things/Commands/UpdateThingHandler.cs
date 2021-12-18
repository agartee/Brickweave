using System.Threading.Tasks;
using AdvancedCqrs.Domain.Things.Models;
using AdvancedCqrs.Domain.Things.Services;
using Brickweave.Cqrs;

namespace AdvancedCqrs.Domain.Things.Commands
{
    public class UpdateThingHandler : ICommandHandler<UpdateThing, Thing>
    {
        private readonly IThingRepository _thingRepository;

        public UpdateThingHandler(IThingRepository thingRepository)
        {
            _thingRepository = thingRepository;
        }

        public async Task<Thing> HandleAsync(UpdateThing command)
        {
            var thing = await _thingRepository.DemandThingAsync(command.Id);

            thing.Name = command.Name;

            await _thingRepository.SaveThingAsync(thing);

            return thing;
        }
    }
}
