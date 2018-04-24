using System;
using System.Threading.Tasks;
using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Commands;
using Brickweave.Samples.Domain.Persons.Models;
using Brickweave.Samples.Domain.Persons.Queries;
using Brickweave.Samples.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Brickweave.Samples.WebApp.Controllers
{
   // [Authorize]
    public class PersonController : Controller
    {
        private readonly IDispatcher _dispatcher;

        public PersonController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpGet, Route("/person/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _dispatcher.DispatchQueryAsync(new GetPerson(new PersonId(id)));

            return Ok(result);
        }

        [HttpPost, Route("/person/new")]
        public async Task<IActionResult> Create([FromBody] CreatePersonRequest request)
        {
            var result = await _dispatcher.DispatchCommandAsync(new CreatePerson(
                PersonId.NewId(), new Name(request.FirstName, request.LastName)));

            return Ok(result);
        }
    }
}
