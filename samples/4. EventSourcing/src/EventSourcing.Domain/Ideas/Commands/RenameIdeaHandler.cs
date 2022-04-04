using System.Threading.Tasks;
using Brickweave.Cqrs;
using EventSourcing.Domain.Ideas.Extensions;
using EventSourcing.Domain.Ideas.Models;
using EventSourcing.Domain.Ideas.Services;

namespace EventSourcing.Domain.Ideas.Commands
{
    public class RenameIdeaHandler : ICommandHandler<RenameIdea, IdeaInfo>
    {
        private readonly IIdeaRepository _ideaRepository;

        public RenameIdeaHandler(IIdeaRepository thingRepository)
        {
            _ideaRepository = thingRepository;
        }

        public async Task<IdeaInfo> HandleAsync(RenameIdea command)
        {
            var idea = await _ideaRepository.DemandIdeaAsync(command.IdeaId);

            if (!command.Name.Equals(idea.Name))
                idea.Rename(command.Name);

            await _ideaRepository.SaveIdeaAsync(idea);

            return idea.ToInfo();
        }
    }
}
