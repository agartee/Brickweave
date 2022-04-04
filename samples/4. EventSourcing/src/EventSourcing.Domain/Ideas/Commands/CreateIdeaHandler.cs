using System.Threading.Tasks;
using Brickweave.Cqrs;
using EventSourcing.Domain.Ideas.Extensions;
using EventSourcing.Domain.Ideas.Models;
using EventSourcing.Domain.Ideas.Services;

namespace EventSourcing.Domain.Ideas.Commands
{
    public class CreateIdeaHandler : ICommandHandler<CreateIdea, IdeaInfo>
    {
        private readonly IIdeaRepository _ideaRepository;

        public CreateIdeaHandler(IIdeaRepository thingRepository)
        {
            _ideaRepository = thingRepository;
        }

        public async Task<IdeaInfo> HandleAsync(CreateIdea command)
        {
            var idea = new Idea(IdeaId.NewId(), command.Name);

            await _ideaRepository.SaveIdeaAsync(idea);

            return idea.ToIdeaInfo();
        }
    }
}
