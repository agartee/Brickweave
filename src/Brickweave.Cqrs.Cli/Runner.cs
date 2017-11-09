using System.Linq;
using System.Threading.Tasks;
using Brickweave.Cqrs.Cli.Factories;

namespace Brickweave.Cqrs.Cli
{
    public class Runner : IRunner
    {
        private readonly IExecutableInfoFactory _executableInfoFactory;
        private readonly IHelpInfoFactory _helpInfoFactory;
        private readonly IExecutableFactory _executableFactory;
        private readonly ICommandExecutor _commandExecutor;
        private readonly IQueryExecutor _queryExecutor;

        public Runner(IExecutableInfoFactory executableInfoFactory, IHelpInfoFactory helpInfoFactory, IExecutableFactory executableFactory, 
            ICommandExecutor commandExecutor, IQueryExecutor queryExecutor)
        {
            _executableInfoFactory = executableInfoFactory;
            _executableFactory = executableFactory;
            _commandExecutor = commandExecutor;
            _queryExecutor = queryExecutor;
            _helpInfoFactory = helpInfoFactory;
        }

        public async Task<object> RunAsync(string[] args)
        {
            if (args.Any(a => a == "--help"))
                return _helpInfoFactory.Create(args);

            var executable = _executableFactory.Create(
                _executableInfoFactory.Create(args));

            var result = executable is ICommand
                ? await _commandExecutor.ExecuteAsync((ICommand)executable)
                : await _queryExecutor.ExecuteAsync((IQuery)executable);

            return result;
        }
    }
}
