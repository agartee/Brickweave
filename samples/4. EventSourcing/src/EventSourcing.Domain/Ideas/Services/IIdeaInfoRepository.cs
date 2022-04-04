using System.Collections.Generic;
using System.Threading.Tasks;
using EventSourcing.Domain.Ideas.Models;

namespace EventSourcing.Domain.Ideas.Services
{
    public interface IIdeaInfoRepository
    {
        Task<IdeaInfo> DemandIdeaInfoAsync(IdeaId id);
        Task<IEnumerable<IdeaInfo>> ListIdeasAsync();
    }
}
