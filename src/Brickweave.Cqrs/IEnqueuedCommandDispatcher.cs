using System.Security.Claims;
using System.Threading.Tasks;

namespace Brickweave.Cqrs
{
    public interface IEnqueuedCommandDispatcher
    {
        Task<object> ExecuteAsync(ICommand command, ClaimsPrincipal user);
    }
}
