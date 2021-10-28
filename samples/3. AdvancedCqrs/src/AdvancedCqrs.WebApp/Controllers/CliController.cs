using System;
using System.Threading.Tasks;
using Brickweave.Cqrs.Cli;
using Brickweave.Cqrs.Cli.Services;
using Brickweave.Cqrs.Models;
using Brickweave.Cqrs.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedCqrs.WebApp.Controllers
{
    public class CliController : Controller
    {
        private readonly ICliDispatcher _cliDispatcher;
        private readonly ICommandStatusRepository _commandStatusRepository;

        public CliController(ICliDispatcher cliDispatcher, ICommandStatusRepository commandStatusRepository)
        {
            _cliDispatcher = cliDispatcher;
            _commandStatusRepository = commandStatusRepository;
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
            var status = await _commandStatusRepository.ReadStatusAsync(commandId);

            if (status is CompletedExecutionStatus completedStatus)
                return Ok(completedStatus.Result);

            if (status is ErrorExecutionStatus errorStatus)
                throw new InvalidOperationException(errorStatus.Message);

            return Accepted($"https://{HttpContext.Request.Host}/cli/status/{commandId}");
        }

    }
}
