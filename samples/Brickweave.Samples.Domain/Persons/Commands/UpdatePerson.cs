using System;
using System.Threading.Tasks;
using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Extensions;
using Brickweave.Samples.Domain.Persons.Models;
using Brickweave.Samples.Domain.Persons.Services;

namespace Brickweave.Samples.Domain.Persons.Commands
{
    public class UpdatePerson : ICommand<PersonInfo>
    {
        /// <summary>
        /// Create a new person.
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="firstName">Person's first name</param>
        /// <param name="lastName">Person's last name</param>
        /// <param name="birthDate">Person's birth date</param>
        public UpdatePerson(PersonId personId, string firstName, string lastName, DateTime? birthDate)
        {
            PersonId = personId;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
        }

        public PersonId PersonId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public DateTime? BirthDate { get; }
    }

    public class UpdatePersonHandler : ICommandHandler<UpdatePerson, PersonInfo>
    {
        private readonly IPersonRepository _personRepository;

        public UpdatePersonHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<PersonInfo> HandleAsync(UpdatePerson command)
        {
            var person = await _personRepository.GetPersonAsync(command.PersonId);

            if (command.FirstName != null)
                person.SetFirstName(command.FirstName);
            if (command.LastName != null)
                person.SetLastName(command.LastName);
            if (command.BirthDate != null)
                person.SetBirthDate(command.BirthDate.Value);

            await _personRepository.SavePersonAsync(person);

            return person.ToInfo();
        }
    }
}
