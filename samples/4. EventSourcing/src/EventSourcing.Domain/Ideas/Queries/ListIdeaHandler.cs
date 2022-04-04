using System.Collections.Generic;
using System.Threading.Tasks;
using Brickweave.Cqrs;
using EventSourcing.Domain.Ideas.Models;
using EventSourcing.Domain.Ideas.Services;

namespace EventSourcing.Domain.Ideas.Queries
{
    public class ListIdeaHandler : IQueryHandler<ListIdeas, IEnumerable<IdeaInfo>>
    {
        private readonly IIdeaInfoRepository _ideaInfoRepository;

        public ListIdeaHandler(IIdeaInfoRepository ideaInfoRepository)
        {
            _ideaInfoRepository = ideaInfoRepository;
        }

        public async Task<IEnumerable<IdeaInfo>> HandleAsync(ListIdeas query)
        {
            return await _ideaInfoRepository.ListIdeasAsync();
        }
    }
}
