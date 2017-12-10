using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Brickweave.Samples.WebApp.Controllers
{
    [Authorize]
    public class IdentityController : ControllerBase
    {
        [HttpGet, Route("/identity")]
        public IActionResult Get()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}
