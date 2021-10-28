using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Brickweave.Cqrs
{
    public interface ICommandDispatcher
    {
        Task<object> ExecuteAsync(ICommand command, Action<Guid> handleCommandEnqueued = null);
        Task<object> ExecuteAsync(ICommand command, ClaimsPrincipal user, Action<Guid> handleCommandEnqueued = null);
        Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command, Action<Guid> handleCommandEnqueued = null);
        Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command, ClaimsPrincipal user, Action<Guid> handleCommandEnqueued = null);
    }
}
