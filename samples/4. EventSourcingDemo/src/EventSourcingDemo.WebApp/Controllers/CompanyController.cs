using Brickweave.Cqrs.Services;
using EventSourcingDemo.Domain.Companies.Commands;
using EventSourcingDemo.Domain.Companies.Models;
using EventSourcingDemo.Domain.Companies.Queries;
using Microsoft.AspNetCore.Mvc;

namespace EventSourcingDemo.WebApp.Controllers
{
    public class CompanyController : Controller
    {
        private readonly IDispatcher _dispatcher;

        public CompanyController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet, Route("/companies/")]
        public async Task<IActionResult> ListAsync()
        {
            var results = await _dispatcher.DispatchQueryAsync(new ListCompanies());

            return View(results);
        }

        [HttpGet, Route("/company/create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, Route("/company/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(CreateCompany command)
        {
            await _dispatcher.DispatchCommandAsync(command);

            return Redirect("/companies");
        }

        [HttpGet, Route("/company/{id}/edit")]
        public async Task<IActionResult> EditAsync(CompanyId id)
        {
            var result = await _dispatcher.DispatchQueryAsync(new GetCompany(id));

            return View(result);
        }

        [HttpPost, Route("/company/{id}/edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(UpdateCompany command)
        {
            await _dispatcher.DispatchCommandAsync(command);

            return Redirect("/companies");
        }

        [HttpPost, Route("/company/{id}/delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAsync(CompanyId id)
        {
            await _dispatcher.DispatchCommandAsync(new DeleteCompany(id));

            return Redirect("/companies");
        }
    }
}
