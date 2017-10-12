using System.Threading.Tasks;
using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Models;
using Brickweave.Samples.Domain.Persons.Services;

namespace Brickweave.Samples.Domain.Persons.Commands
{
    public class CreatePersonHandler : ICommandHandler<CreatePerson, PersonInfo>
    {
        private readonly IPersonRepository _personRepository;

        public CreatePersonHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<PersonInfo> HandleAsync(CreatePerson command)
        {
            var person = new Person
            {
                Id = command.PersonId,
                FirstName = command.FirstName,
                LastName = command.LastName
            };

            await _personRepository.SaveAsync(person);

            return new PersonInfo(person.Id, person.FirstName, person.LastName);
        }
    }
}