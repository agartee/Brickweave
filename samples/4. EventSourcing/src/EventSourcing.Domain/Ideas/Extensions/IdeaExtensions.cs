using EventSourcing.Domain.Ideas.Models;

namespace EventSourcing.Domain.Ideas.Extensions
{
    public static class IdeaExtensions
    {
        public static IdeaInfo ToInfo(this Idea idea)
        {
            return new IdeaInfo(idea.Id, idea.Name);
        }
    }
}
