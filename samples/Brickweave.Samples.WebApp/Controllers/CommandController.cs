using System.Threading;
using System.Threading.Tasks;
using Brickweave.Cqrs.Cli;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Brickweave.Samples.WebApp.Controllers
{
    [Authorize]
    public class CommandController : Controller
    {
        private readonly ICliDispatcher _cliDispatcher;

        public CommandController(ICliDispatcher cliDispatcher)
        {
            _cliDispatcher = cliDispatcher;
        }

        [HttpPost, Route("/command/run")]
        public async Task<IActionResult> Run([FromBody]string commandText, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _cliDispatcher.DispatchAsync(commandText, cancellationToken);

            return Ok(result);
        }
    }
}
