using System.Threading.Tasks;
using BasicCqrs.Domain.People.Models;
using BasicCqrs.Domain.People.Services;
using Brickweave.Cqrs;

namespace BasicCqrs.Domain.People.Commands
{
    public class CreatePersonHandler : ICommandHandler<CreatePerson, Person>
    {
        private readonly IPersonRepository personRepository;

        public CreatePersonHandler(IPersonRepository personRepository)
        {
            this.personRepository = personRepository;
        }

        public async Task<Person> HandleAsync(CreatePerson command)
        {
            var person = new Person(PersonId.NewId());

            person.FirstName = command.FirstName;
            person.LastName = command.LastName;

            await personRepository.SavePersonAsync(person);

            return person;
        }
    }
}
