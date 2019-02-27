using System.Threading.Tasks;
using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Extensions;
using Brickweave.Samples.Domain.Persons.Models;
using Brickweave.Samples.Domain.Persons.Services;

namespace Brickweave.Samples.Domain.Persons.Commands
{
    public class AddSinglePersonAttributeHandler : ICommandHandler<AddSinglePersonAttribute, PersonInfo>
    {
        private readonly IPersonRepository _personRepository;

        public AddSinglePersonAttributeHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<PersonInfo> HandleAsync(AddSinglePersonAttribute command)
        {
            var person = await _personRepository.GetPersonAsync(command.PersonId);

            person.AddAttribute(command.Key, command.Value);

            await _personRepository.SavePersonAsync(person);

            return person.ToInfo();
        }
    }
}
