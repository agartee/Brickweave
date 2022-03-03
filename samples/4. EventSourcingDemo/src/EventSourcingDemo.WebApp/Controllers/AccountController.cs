using Brickweave.Cqrs.Services;
using EventSourcingDemo.Domain.Accounts.Queries;
using Microsoft.AspNetCore.Mvc;

namespace EventSourcingDemo.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IDispatcher _dispatcher;

        public AccountController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet, Route("/accounts/")]
        public async Task<IActionResult> ListAsync()
        {
            var results = await _dispatcher.DispatchQueryAsync(new ListAccounts());

            return View(results);
        }
    }
}
