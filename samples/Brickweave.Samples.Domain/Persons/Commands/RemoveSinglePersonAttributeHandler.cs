using System.Threading.Tasks;
using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Extensions;
using Brickweave.Samples.Domain.Persons.Models;
using Brickweave.Samples.Domain.Persons.Services;

namespace Brickweave.Samples.Domain.Persons.Commands
{
    public class RemoveSinglePersonAttributeHandler : ICommandHandler<RemoveSinglePersonAttribute, PersonInfo>
    {
        private readonly IPersonRepository _personRepository;

        public RemoveSinglePersonAttributeHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<PersonInfo> HandleAsync(RemoveSinglePersonAttribute command)
        {
            var person = await _personRepository.GetPersonAsync(command.PersonId);

            person.RemoveAttribute(command.Key);

            await _personRepository.SavePersonAsync(person);

            return person.ToInfo();
        }
    }
}
