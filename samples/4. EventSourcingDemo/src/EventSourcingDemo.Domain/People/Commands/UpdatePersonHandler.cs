using Brickweave.Cqrs;
using EventSourcingDemo.Domain.People.Extensions;
using EventSourcingDemo.Domain.People.Models;
using EventSourcingDemo.Domain.People.Services;

namespace EventSourcingDemo.Domain.People.Commands
{
    public class UpdatePersonHandler : ICommandHandler<UpdatePerson, PersonInfo>
    {
        private readonly IPersonRepository _personRepository;

        public UpdatePersonHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<PersonInfo> HandleAsync(UpdatePerson command)
        {
            var person = await _personRepository.DemandPersonAsync(command.PersonId);

            person.Name = command.Name;

            await _personRepository.SavePersonAsync(person);

            return person.ToPersonInfo();
        }
    }
}
