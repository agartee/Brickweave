using System.Threading.Tasks;
using EventSourcing.Domain.Ideas.Models;

namespace EventSourcing.Domain.Ideas.Services
{
    public interface IIdeaRepository
    {
        Task SaveIdeaAsync(Idea idea);
        Task<Idea> DemandIdeaAsync(IdeaId id);   
    }
}
