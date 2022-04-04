using System.Collections.Generic;
using Brickweave.Cqrs;
using EventSourcing.Domain.Ideas.Models;

namespace EventSourcing.Domain.Ideas.Queries
{
    public class ListIdeas : IQuery<IEnumerable<IdeaInfo>>
    {
        public ListIdeas() 
        { 
        }
    }
}
