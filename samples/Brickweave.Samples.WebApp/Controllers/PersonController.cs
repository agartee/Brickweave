﻿using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Commands;
using Brickweave.Samples.Domain.Persons.Models;
using Brickweave.Samples.Domain.Persons.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Brickweave.Samples.WebApp.Controllers
{
    //[Authorize]
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
        public async Task<IActionResult> Create([FromBody] CreatePerson command)
        {
            var result = await _dispatcher.DispatchCommandAsync(command);

            return Ok(result);
        }

        [HttpPost, Route("/person/addPhones")]
        public async Task<IActionResult> AddPhone([FromBody] AddPersonPhones command)
        {
            var result = await _dispatcher.DispatchCommandAsync(command);

            return Ok(result);
        }
    }
}
