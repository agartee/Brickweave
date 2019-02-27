using System.Linq;
using System.Threading.Tasks;
using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Extensions;
using Brickweave.Samples.Domain.Persons.Models;
using Brickweave.Samples.Domain.Persons.Services;

namespace Brickweave.Samples.Domain.Persons.Commands
{
    public class AddMultiplePersonAttributesHandler : ICommandHandler<AddMultiplePersonAttributes, PersonInfo>
    {
        private readonly IPersonRepository _personRepository;

        public AddMultiplePersonAttributesHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<PersonInfo> HandleAsync(AddMultiplePersonAttributes command)
        {
            var person = await _personRepository.GetPersonAsync(command.PersonId);

            command.Attributes.ToList().ForEach(kvp =>
            {
                kvp.Value.ForEach(v =>
                {
                    person.AddAttribute(kvp.Key, v);
                });
            });

            await _personRepository.SavePersonAsync(person);

            return person.ToInfo();
        }
    }
}
