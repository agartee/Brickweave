﻿using System.Threading.Tasks;
using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Models;
using Brickweave.Samples.Domain.Persons.Services;

namespace Brickweave.Samples.Domain.Persons.Commands
{
    public class AddPersonPhoneHandler : ICommandHandler<AddPersonPhone, PersonInfo>
    {
        private readonly IPersonRepository _personRepository;

        public AddPersonPhoneHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<PersonInfo> HandleAsync(AddPersonPhone command)
        {
            var person = await _personRepository.GetPersonAsync(command.Id);
            person.AddPhone(PhoneId.NewId(), command.PhoneNumber);

            await _personRepository.SavePersonAsync(person);

            return person.ToInfo();
        }
    }
}
