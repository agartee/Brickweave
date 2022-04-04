using Brickweave.Cqrs;
using EventSourcing.Domain.Common.Models;
using EventSourcing.Domain.Ideas.Models;

namespace EventSourcing.Domain.Ideas.Commands
{
    public class CreateIdea : ICommand<IdeaInfo>
    {
        public CreateIdea(Name name)
        {
            Name = name;
        }

        public Name Name { get; }
    }
}
