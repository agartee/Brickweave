using System.Threading.Tasks;
using Brickweave.Cqrs;
using Brickweave.Cqrs.Cli.Factories;
using Brickweave.Cqrs.Cli.Parsers;
using Microsoft.AspNetCore.Mvc;

namespace Brickweave.Samples.WebApp.Controllers
{
    public class CommandController : Controller
    {
        private readonly IArgParser _parser;
        private readonly IExecutableFactory _executableFactory;
        private readonly ICommandExecutor _commandExecutor;
        private readonly IQueryExecutor _queryExecutor;

        public CommandController(IArgParser parser, IExecutableFactory executableFactory, 
            ICommandExecutor commandExecutor, IQueryExecutor queryExecutor)
        {
            _parser = parser;
            _executableFactory = executableFactory;
            _commandExecutor = commandExecutor;
            _queryExecutor = queryExecutor;
        }

        [HttpPost, Route("/command/run")]
        public async Task<IActionResult> Run([FromBody]string payload)
        {
            var executableInfo = _parser.Parse(payload.Split(' '));
            var executable = _executableFactory.Create(executableInfo);

            var result = executable is ICommand
                ? await _commandExecutor.ExecuteAsync((ICommand) executable)
                : await _queryExecutor.ExecuteAsync((IQuery)executable);

            return Ok(result);
        }
    }
}
