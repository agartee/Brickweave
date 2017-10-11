using System;
using System.Threading.Tasks;
using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Commands;
using Brickweave.Samples.Domain.Persons.Queries;
using Brickweave.Samples.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Brickweave.Samples.WebApp.Controllers
{
    public class PersonController : Controller
    {
        private readonly ICommandProcessor _commandProcessor;
        private readonly IQueryProcessor _queryProcessor;

        public PersonController(ICommandProcessor commandProcessor, IQueryProcessor queryProcessor)
        {
            _commandProcessor = commandProcessor;
            _queryProcessor = queryProcessor;
        }

        [HttpGet, Route("/person/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _queryProcessor.ProcessAsync(new GetPerson(id));

            return Ok(result);
        }

        [HttpPost, Route("/person/new")]
        public async Task<IActionResult> Create([FromBody] CreatePersonRequest request)
        {
            var result = await _commandProcessor.ProcessAsync(new CreatePerson(
                Guid.NewGuid(), request.FirstName, request.LastName));

            return Ok(result);
        }
    }
}
