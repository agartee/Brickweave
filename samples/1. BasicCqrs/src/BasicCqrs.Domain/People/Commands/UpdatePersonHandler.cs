using System.Threading.Tasks;
using BasicCqrs.Domain.People.Models;
using BasicCqrs.Domain.People.Services;
using Brickweave.Cqrs;

namespace BasicCqrs.Domain.People.Commands
{
    public class UpdatePersonHandler : ICommandHandler<UpdatePerson, Person>
    {
        private readonly IPersonRepository personRepository;

        public UpdatePersonHandler(IPersonRepository personRepository)
        {
            this.personRepository = personRepository;
        }

        public async Task<Person> HandleAsync(UpdatePerson command)
        {
            var person = await personRepository.GetPersonAsync(command.Id);

            if (command.FirstName != null)
                person.FirstName = command.FirstName;

            if (command.LastName != null)
                person.LastName = command.LastName;

            await personRepository.SavePersonAsync(person);

            return person;
        }
    }
}
