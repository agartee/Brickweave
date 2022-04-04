using Brickweave.Cqrs;
using EventSourcing.Domain.Ideas.Models;

namespace EventSourcing.Domain.Ideas.Queries
{
    public class GetIdea : IQuery<IdeaInfo>
    {
        public GetIdea(IdeaId ideaId)
        {
            IdeaId = ideaId;
        }

        public IdeaId IdeaId { get; }
    }
}
