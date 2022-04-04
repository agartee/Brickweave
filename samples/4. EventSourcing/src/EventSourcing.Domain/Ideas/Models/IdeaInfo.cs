using EventSourcing.Domain.Common.Models;

namespace EventSourcing.Domain.Ideas.Models
{
    public class IdeaInfo
    {
        public IdeaInfo(IdeaId id, Name name)
        {
            Id = id;
            Name = name;
        }

        public IdeaId Id { get; }
        public Name Name { get; }
    }
}
