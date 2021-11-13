using Microsoft.AspNetCore.Mvc;

namespace AdvancedCqrs.WebApp.Controllers
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
