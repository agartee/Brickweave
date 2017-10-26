using System.Threading.Tasks;
using Brickweave.Cqrs;
using Brickweave.Messaging;
using Brickweave.Samples.Domain.Persons.Events;
using Brickweave.Samples.Domain.Persons.Models;
using Brickweave.Samples.Domain.Persons.Services;

namespace Brickweave.Samples.Domain.Persons.Commands
{
    public class CreatePersonHandler : ICommandHandler<CreatePerson, PersonInfo>
    {
        private readonly IPersonRepository _personRepository;
        private readonly IDomainMessenger _messenger;

        public CreatePersonHandler(IPersonRepository personRepository, IDomainMessenger messenger)
        {
            _personRepository = personRepository;
            _messenger = messenger;
        }

        public async Task<PersonInfo> HandleAsync(CreatePerson command)
        {
            var person = new Person(command.PersonId, command.Name);

            await _personRepository.SavePersonAsync(person);
            await _messenger.SendAsync(new PersonCreated(person.Id.Value, person.Name.FirstName, person.Name.LastName));

            return person.ToInfo();
        }
    }
}