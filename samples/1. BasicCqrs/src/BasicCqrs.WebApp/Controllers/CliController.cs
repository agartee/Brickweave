using System.Threading.Tasks;
using Brickweave.Cqrs.Cli.Formatters;
using Brickweave.Cqrs.Cli.Models;
using Brickweave.Cqrs.Cli.Services;
using Microsoft.AspNetCore.Mvc;

namespace BasicCqrs.WebApp.Controllers
{
    public class CliController : Controller
    {
        private readonly ICliDispatcher _cliDispatcher;

        public CliController(ICliDispatcher cliDispatcher)
        {
            _cliDispatcher = cliDispatcher;
        }

        [HttpPost, Route("/cli/run")]
        public async Task<IActionResult> RunAsync([FromBody] string commandText)
        {
            // note: Additional steps required to setup your own application
            // for the CLI dispatcher:
            // 1. Add an XML documentation file to your domain libraries (see
            //    README.md in the Brickweave repository for instructions).
            // 2. Recommended: Disable compiler warnings for
            //    missing-documentation (codes 1701, 1702, and 1591) to
            //    prevent Visual Studio from warning that documentation is
            //    missing from all constructors and methods.
            // 3. Create a JSON file to provide help documentation for your
            //    domain models (e.g. cli-categories.json in this demo) are
            //    wire it up in Startup.cs.

            var result = await _cliDispatcher.DispatchAsync(commandText);

            var value = result is HelpInfo info
                ? SimpleHelpFormatter.Format(info) : result;

            return Ok(value);
        }
    }
}
