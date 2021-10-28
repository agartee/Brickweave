using System.Threading.Tasks;
using BasicMessaging.Domain.Places.Commands;
using BasicMessaging.WebApp.Models;
using Brickweave.Cqrs;
using Brickweave.Cqrs.Services;
using Microsoft.AspNetCore.Mvc;

namespace BasicMessaging.WebApp.Controllers
{
    public class PlaceController : Controller
    {
        private readonly IDispatcher _dispatcher;

        public PlaceController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet, Route("/place/create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, Route("/place/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(PlaceViewModel viewModel)
        {
            await _dispatcher.DispatchCommandAsync(new CreatePlace(
                viewModel.Name));

            return View();
        }
    }
}
