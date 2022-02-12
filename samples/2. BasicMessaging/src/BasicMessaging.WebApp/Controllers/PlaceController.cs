using System.Threading.Tasks;
using BasicMessaging.Domain.Places.Commands;
using BasicMessaging.Domain.Places.Queries;
using BasicMessaging.WebApp.Models;
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

        [HttpGet, Route("/places")]
        public async Task<IActionResult> ListAsync()
        {
            var results = await _dispatcher.DispatchQueryAsync(new ListPlaces());

            return View(results);
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

            return Redirect("/places");
        }
    }
}
