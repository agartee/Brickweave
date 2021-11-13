using Microsoft.AspNetCore.Mvc;

namespace BasicCqrs.WebApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet, Route("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
