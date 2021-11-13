using System;
using System.Threading.Tasks;
using AdvancedCqrs.Domain.Things.Commands;
using AdvancedCqrs.WebApp.Models;
using Brickweave.Cqrs.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedCqrs.WebApp.Controllers
{
    public class ThingController : Controller
    {
        private readonly IDispatcher _dispatcher;

        public ThingController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet, Route("/thing/create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, Route("/thing/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(ThingViewModel viewModel)
        {
            Guid? commandId = null;
            await _dispatcher.DispatchCommandAsync(new CreateThing(viewModel.Name), 
                id => commandId = id);

            return Redirect("/");
        }
    }
}
