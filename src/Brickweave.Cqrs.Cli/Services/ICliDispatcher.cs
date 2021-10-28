using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Brickweave.Cqrs.Cli.Services
{
    public interface ICliDispatcher
    {
        Task<object> DispatchAsync(string commandText, Action<Guid> handleCommandEnqueued = null);
        Task<object> DispatchAsync(string commandText, ClaimsPrincipal user, Action<Guid> handleCommandEnqueued = null);
    }
}
