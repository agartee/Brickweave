using System;
using System.Linq;
using System.Threading.Tasks;
using BasicCqrs.Domain.People.Commands;
using BasicCqrs.Domain.People.Models;
using BasicCqrs.Domain.People.Queries;
using BasicCqrs.WebApp.Extensions;
using BasicCqrs.WebApp.Models;
using Brickweave.Cqrs.Services;
using Microsoft.AspNetCore.Mvc;

namespace BasicCqrs.WebApp.Controllers
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

            return View(results
                .Select(p => p.ToViewModel())
                .ToList());
        }

        [HttpGet, Route("/person/create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, Route("/person/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(PersonViewModel viewModel)
        {
            await _dispatcher.DispatchCommandAsync(new CreatePerson(
                viewModel.FirstName,
                viewModel.LastName));

            return Redirect("/people");
        }

        [HttpGet, Route("/person/{id}/edit")]
        public async Task<IActionResult> EditAsync([FromRoute] Guid id)
        {
            var result = await _dispatcher.DispatchQueryAsync(new GetPerson(
                new PersonId(id)));

            return View(result.ToViewModel());
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
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _dispatcher.DispatchCommandAsync(new DeletePerson(
                new PersonId(id)));

            return Redirect("/people");
        }
    }
}
