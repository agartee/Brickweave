using Microsoft.AspNetCore.Mvc;

namespace EventSourcing.WebApp.Controllers
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
