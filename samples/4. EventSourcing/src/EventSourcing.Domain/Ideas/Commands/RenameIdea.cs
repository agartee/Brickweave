using Brickweave.Cqrs;
using EventSourcing.Domain.Common.Models;
using EventSourcing.Domain.Ideas.Models;

namespace EventSourcing.Domain.Ideas.Commands
{
    public class RenameIdea : ICommand<IdeaInfo>
    {
        public RenameIdea(IdeaId ideaId, Name name)
        {
            IdeaId = ideaId;
            Name = name;
        }

        public IdeaId IdeaId { get; }
        public Name Name { get; }
    }
}
