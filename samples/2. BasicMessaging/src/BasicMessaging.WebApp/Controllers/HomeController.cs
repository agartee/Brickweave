using Microsoft.AspNetCore.Mvc;

namespace BasicMessaging.WebApp.Controllers
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
