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
            // note: This endpoint follows a more traditional view-model
            // pattern of page binding. In this case, the view-model also
            // contains an Id property, because it is reused in the
            // Person-List view, that is ignored in this context which is kind
            // of gross. Another path with this strategy might be to create a
            // view-model object that is specific to this view and therefore
            // does not carry the Id property.

            await _dispatcher.DispatchCommandAsync(new CreatePerson(
                viewModel.FirstName,
                viewModel.LastName));

            return Redirect("/people");
        }

        [HttpGet, Route("/person/{id}/edit")]
        public async Task<IActionResult> EditAsync(PersonId id)
        {
            // note: Because this endpoint is configured to use the
            // IdModelBinder, the [FromQuery] attribute is no longer required.

            var result = await _dispatcher.DispatchQueryAsync(new GetPerson(id));

            return View(result.ToViewModel());
        }

        [HttpPost, Route("/person/{id}/edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(UpdatePerson command)
        {
            // note: The source of the Id property for the command is the query
            // string, but it does not need to be referenced in the endpoint
            // parameters because we're using a custom model-binder (see
            // Startup.cs).

            // note: The UpdatePerson command class does not have a constructor
            // and is therefore able to be used directly from the built-in
            // model binders without needing a custom one. The drawback here
            // is that no validation can be enforced at the class level (e.g.
            // null checks on the properties) in a constructor. Property
            // validation attributes (e.g. [Required]) can be used, but are
            // ignored if the class is instantiated outside of a model-binding
            // scenario.

            await _dispatcher.DispatchCommandAsync(command);

            return Redirect("/people");
        }

        [HttpPost, Route("/person/{id}/delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAsync(PersonId id)
        {
            // note: Because this endpoint is configured to use the
            // IdModelBinder, the [FromQuery] attribute is no longer required.

            await _dispatcher.DispatchCommandAsync(new DeletePerson(id));

            return Redirect("/people");
        }
    }
}
