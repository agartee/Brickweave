using System.Threading.Tasks;
using Brickweave.Cqrs.Cli.Factories;
using Brickweave.Cqrs.Cli.Parsers;

namespace Brickweave.Cqrs.Cli
{
    public class Runner : IRunner
    {
        private readonly IArgParser _parser;
        private readonly IExecutableFactory _executableFactory;
        private readonly ICommandExecutor _commandExecutor;
        private readonly IQueryExecutor _queryExecutor;

        public Runner(IArgParser parser, IExecutableFactory executableFactory, 
            ICommandExecutor commandExecutor, IQueryExecutor queryExecutor)
        {
            _parser = parser;
            _executableFactory = executableFactory;
            _commandExecutor = commandExecutor;
            _queryExecutor = queryExecutor;
        }

        public async Task<object> Run(string[] args)
        {
            var executableInfo = _parser.Parse(args);
            var executable = _executableFactory.Create(executableInfo);

            var result = executable is ICommand
                ? await _commandExecutor.ExecuteAsync((ICommand)executable)
                : await _queryExecutor.ExecuteAsync((IQuery)executable);

            return result;
        }
    }
}
