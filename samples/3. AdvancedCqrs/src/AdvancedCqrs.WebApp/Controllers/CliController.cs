using System.Linq;
using System.Threading.Tasks;
using Brickweave.Cqrs;
using Brickweave.Cqrs.Cli.Extensions;
using Brickweave.Cqrs.Cli.Factories;
using Brickweave.Cqrs.Cli.Formatters;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedCqrs.WebApp.Controllers
{
    public class CliController : Controller
    {
        private readonly IDispatcher _dispatcher;
        private readonly IHelpInfoFactory _helpInfoFactory;
        private readonly IExecutableInfoFactory _executableInfoFactory;
        private readonly IExecutableFactory _executableFactory;

        public CliController(IDispatcher dispatcher, IHelpInfoFactory helpInfoFactory, IExecutableInfoFactory executableInfoFactory, 
            IExecutableFactory executableFactory)
        {
            _dispatcher = dispatcher;
            _helpInfoFactory = helpInfoFactory;
            _executableInfoFactory = executableInfoFactory;
            _executableFactory = executableFactory;
        }

        [HttpPost, Route("/cli/run")]
        public async Task<IActionResult> RunAsync([FromBody] string commandText)
        {
            var args = commandText.ParseCommandText();

            if (args.Any(a => a == "--help"))
            {
                var helpInfo = _helpInfoFactory.Create(args);
                return Ok(SimpleHelpFormatter.Format(helpInfo));
            }

            var executableInfo = _executableInfoFactory.Create(args);
            var executable = _executableFactory.Create(executableInfo);

            var result = executable is ICommand
                ? await _dispatcher.DispatchCommandAsync((ICommand)executable)
                : await _dispatcher.DispatchQueryAsync((IQuery)executable);

            return Ok(result);
        }
    }
}
