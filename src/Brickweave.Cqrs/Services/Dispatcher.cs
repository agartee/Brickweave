using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Brickweave.Cqrs.Services
{
    public class Dispatcher : IDispatcher
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public Dispatcher(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        public async Task<object> DispatchCommandAsync(ICommand command, Action<Guid> handleCommandEnqueued = null)
        {
            return await DispatchCommandAsync(command, null, handleCommandEnqueued);
        }

        public async Task<object> DispatchCommandAsync(ICommand command, ClaimsPrincipal user = null, Action<Guid> handleCommandEnqueued = null)
        {
            return await _commandDispatcher.ExecuteAsync(command, user, handleCommandEnqueued);
        }

        public async Task<TResult> DispatchCommandAsync<TResult>(ICommand<TResult> command, Action<Guid> handleCommandEnqueued = null)
        {
            return await DispatchCommandAsync(command, null, handleCommandEnqueued);
        }

        public async Task<TResult> DispatchCommandAsync<TResult>(ICommand<TResult> command, ClaimsPrincipal user = null, Action<Guid> handleCommandEnqueued = null)
        {
            return await _commandDispatcher.ExecuteAsync(command, user, handleCommandEnqueued);
        }

        public async Task<object> DispatchQueryAsync(IQuery query, ClaimsPrincipal user = null)
        {
            return await _queryDispatcher.ExecuteAsync(query, user);
        }

        public async Task<TResult> DispatchQueryAsync<TResult>(IQuery<TResult> query, ClaimsPrincipal user = null)
        {
            return await _queryDispatcher.ExecuteAsync(query, user);
        }
    }
}
