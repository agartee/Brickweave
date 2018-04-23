using System.Security.Claims;
using System.Threading.Tasks;

namespace Brickweave.Cqrs
{
    public class Dispatcher : IDispatcher
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly IProjectionDispatcher _projectionDispatcher;

        public Dispatcher(ICommandDispatcher commandDispatcher,
            IQueryDispatcher queryDispatcher,
            IProjectionDispatcher projectionDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
            _projectionDispatcher = projectionDispatcher;
        }

        public async Task<object> DispatchCommandAsync(ICommand command, ClaimsPrincipal user = null)
        {
            var commandResult = await _commandDispatcher.ExecuteAsync(command, user);

            await _projectionDispatcher.ExecuteAsync(command);

            return commandResult;  
        }

        public async Task<TResult> DispatchCommandAsync<TResult>(ICommand<TResult> command, ClaimsPrincipal user = null)
        {
            var commandResult = await _commandDispatcher.ExecuteAsync(command, user);

            await _projectionDispatcher.ExecuteAsync(command);

            return commandResult;
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