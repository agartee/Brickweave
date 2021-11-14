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
