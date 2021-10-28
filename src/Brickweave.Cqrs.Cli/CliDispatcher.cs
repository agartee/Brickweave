using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Brickweave.Cqrs.Cli.Extensions;
using Brickweave.Cqrs.Cli.Factories;

namespace Brickweave.Cqrs.Cli
{
    public class CliDispatcher : ICliDispatcher
    {
        private readonly IExecutableInfoFactory _executableInfoFactory;
        private readonly IHelpInfoFactory _helpInfoFactory;
        private readonly IExecutableFactory _executableFactory;
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public CliDispatcher(IExecutableInfoFactory executableInfoFactory, IHelpInfoFactory helpInfoFactory, 
            IExecutableFactory executableFactory, ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _executableInfoFactory = executableInfoFactory;
            _executableFactory = executableFactory;
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
            _helpInfoFactory = helpInfoFactory;
        }

        public async Task<object> DispatchAsync(string commandText, Action<Guid> handleCommandEnqueued = null)
        {
            return await DispatchAsync(commandText, null, handleCommandEnqueued);
        }

        public async Task<object> DispatchAsync(string commandText, ClaimsPrincipal user, Action<Guid> handleCommandEnqueued = null)
        {
            var args = commandText.ParseCommandText();

            if (args.Any(a => a == "--help"))
                return _helpInfoFactory.Create(args);

            var executableInfo = _executableInfoFactory.Create(args);
            var executable = _executableFactory.Create(executableInfo);

            var result = executable is ICommand
                ? await _commandDispatcher.ExecuteAsync((ICommand)executable, user, handleCommandEnqueued)
                : await _queryDispatcher.ExecuteAsync((IQuery)executable, user);

            return result;
        }
    }
}
