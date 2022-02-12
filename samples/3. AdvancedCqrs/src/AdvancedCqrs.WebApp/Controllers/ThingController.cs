using System;
using System.Threading.Tasks;
using AdvancedCqrs.Domain.Things.Commands;
using AdvancedCqrs.Domain.Things.Models;
using AdvancedCqrs.Domain.Things.Queries;
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

        [HttpGet, Route("/things")]
        public async Task<IActionResult> ListAsync()
        {
            var results = await _dispatcher.DispatchQueryAsync(new ListThings());

            return View(results);
        }

        [HttpGet, Route("/thing/create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, Route("/thing/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(CreateThing command)
        {
            Guid? commandId = null;
            await _dispatcher.DispatchCommandAsync(command, 
                id => commandId = id);

            return Redirect("/things");
        }

        [HttpGet, Route("/thing/{id}/edit")]
        public async Task<IActionResult> EditAsync(ThingId id)
        {
            var result = await _dispatcher.DispatchQueryAsync(new GetThing(id));

            return View(result);
        }

        [HttpPost, Route("/thing/{id}/edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(UpdateThing command)
        {
            await _dispatcher.DispatchCommandAsync(command);

            return Redirect("/things");
        }
    }
}
