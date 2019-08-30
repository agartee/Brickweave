using System.Threading.Tasks;
using Brickweave.Cqrs.Cli;
using Brickweave.Cqrs.Cli.Formatters;
using Brickweave.Cqrs.Cli.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Brickweave.Samples.WebApp.Controllers
{
    [Authorize]
    public class CliController : Controller
    {
        private readonly ICliDispatcher _cliDispatcher;

        public CliController(ICliDispatcher cliDispatcher)
        {
            _cliDispatcher = cliDispatcher;
        }

        [HttpPost, Route("/cli/run")]
        public async Task<IActionResult> Run([FromBody]string commandText)
        {
            var result = await _cliDispatcher.DispatchAsync(commandText);

            var value = result is HelpInfo info
                ? SimpleHelpFormatter.Format(info) : result;

            return Ok(value);
        }
    }
}
