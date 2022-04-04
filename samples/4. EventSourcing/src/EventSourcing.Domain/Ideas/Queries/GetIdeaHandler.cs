using System.Threading.Tasks;
using Brickweave.Cqrs;
using EventSourcing.Domain.Ideas.Models;
using EventSourcing.Domain.Ideas.Services;

namespace EventSourcing.Domain.Ideas.Queries
{
    public class GetIdeaHandler : IQueryHandler<GetIdea, IdeaInfo>
    {
        private readonly IIdeaInfoRepository _ideaInfoRepository;

        public GetIdeaHandler(IIdeaInfoRepository ideaInfoRepository)
        {
            _ideaInfoRepository = ideaInfoRepository;
        }

        public async Task<IdeaInfo> HandleAsync(GetIdea query)
        {
            return await _ideaInfoRepository.DemandIdeaInfoAsync(query.IdeaId);
        }
    }
}
