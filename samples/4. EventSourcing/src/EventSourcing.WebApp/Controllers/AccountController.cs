using Brickweave.Cqrs.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventSourcing.WebApp.Controllers
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

            return View(results
                .Select(p => p.ToViewModel())
                .ToList());
        }
    }
}
