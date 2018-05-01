using System.Linq;
using System.Threading;
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

        public async Task<object> DispatchAsync(string commandText, CancellationToken cancellationToken = default(CancellationToken))
        {
            var args = commandText.ParseCommandText();

            if (args.Any(a => a == "--help"))
                return _helpInfoFactory.Create(args);

            var executableInfo = _executableInfoFactory.Create(args);
            var executable = _executableFactory.Create(
                executableInfo);

            var result = executable is ICommand
                ? await _commandDispatcher.ExecuteAsync((ICommand)executable, cancellationToken: cancellationToken)
                : await _queryDispatcher.ExecuteAsync((IQuery)executable, cancellationToken: cancellationToken);

            return result;
        }
    }
}
