using System.Threading.Tasks;
using Brickweave.Cqrs.Cli;
using Brickweave.Cqrs.Cli.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Brickweave.Samples.WebApp.Controllers
{
    [Authorize]
    public class CommandController : Controller
    {
        private readonly IRunner _runner;

        public CommandController(IRunner runner)
        {
            _runner = runner;
        }

        [HttpPost, Route("/command/run")]
        public async Task<IActionResult> Run([FromBody]string payload)
        {
            var result = await _runner.RunAsync(payload.SplitOnSpacesWithQuotes());

            return Ok(result);
        }
    }
}
