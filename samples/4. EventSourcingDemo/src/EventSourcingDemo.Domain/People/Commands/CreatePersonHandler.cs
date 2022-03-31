using Brickweave.Cqrs;
using EventSourcingDemo.Domain.People.Extensions;
using EventSourcingDemo.Domain.People.Models;
using EventSourcingDemo.Domain.People.Services;

namespace EventSourcingDemo.Domain.People.Commands
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
            var person = new Person(PersonId.NewId(), command.Name);

            await _personRepository.SavePersonAsync(person);

            return person.ToPersonInfo();
        }
    }
}
