using System;
using System.Threading.Tasks;
using Brickweave.Cqrs.Cli.Services;
using Brickweave.Cqrs.Models;
using Brickweave.Cqrs.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedCqrs.WebApp.Controllers
{
    public class CliController : Controller
    {
        private readonly ICliDispatcher _cliDispatcher;
        private readonly ICommandStatusProvider _commandStatusProvider;

        public CliController(ICliDispatcher cliDispatcher, ICommandStatusProvider commandStatusProvider)
        {
            _cliDispatcher = cliDispatcher;
            _commandStatusProvider = commandStatusProvider;
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

            // note: In this demo, long-running commands are enabled and the
            // logic needs to be slightly different than in the BasicCqrs demo.
            // The CLI client (PowerShell script) is coded to check for
            // redirects and will loop, checking the status of the command
            // execution until it is completed.

            Guid? commandId = null;
            var result = await _cliDispatcher.DispatchAsync(commandText, id => commandId = id);

            if(commandId != null)
                return Accepted($"https://{HttpContext.Request.Host}/cli/status/{commandId}");

            return Ok(result);
        }

        [HttpGet, Route("/cli/status/{commandId}")]
        public async Task<IActionResult> GetStatus(Guid commandId)
        {
            var status = await _commandStatusProvider.GetStatusAsync(commandId);

            if (status is CommandCompletedExecutionStatus completedStatus)
                return Ok(completedStatus.Result);

            if (status is CommandErrorExecutionStatus errorStatus)
                throw new InvalidOperationException(errorStatus.Exception.ToString());

            return Accepted($"https://{HttpContext.Request.Host}/cli/status/{commandId}");
        }
    }
}
