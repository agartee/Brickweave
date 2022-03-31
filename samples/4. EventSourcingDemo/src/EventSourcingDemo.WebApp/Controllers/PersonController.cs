using Brickweave.Cqrs.Services;
using EventSourcingDemo.Domain.People.Commands;
using EventSourcingDemo.Domain.People.Models;
using EventSourcingDemo.Domain.People.Queries;
using Microsoft.AspNetCore.Mvc;

namespace EventSourcingDemo.WebApp.Controllers
{
    public class PersonController : Controller
    {
        private readonly IDispatcher _dispatcher;

        public PersonController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet, Route("/people/")]
        public async Task<IActionResult> ListAsync()
        {
            var results = await _dispatcher.DispatchQueryAsync(new ListPeople());

            return View(results);
        }

        [HttpGet, Route("/person/create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, Route("/person/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(CreatePerson command)
        {
            await _dispatcher.DispatchCommandAsync(command);

            return Redirect("/people");
        }

        [HttpGet, Route("/person/{id}/edit")]
        public async Task<IActionResult> EditAsync(PersonId id)
        {
            var result = await _dispatcher.DispatchQueryAsync(new GetPerson(id));

            return View(result);
        }

        [HttpPost, Route("/person/{id}/edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(UpdatePerson command)
        {
            await _dispatcher.DispatchCommandAsync(command);

            return Redirect("/people");
        }

        [HttpPost, Route("/person/{id}/delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAsync(PersonId id)
        {
            await _dispatcher.DispatchCommandAsync(new DeletePerson(id));

            return Redirect("/people");
        }
    }
}
