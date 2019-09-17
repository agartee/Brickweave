using System.Threading.Tasks;
using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Extensions;
using Brickweave.Samples.Domain.Persons.Models;
using Brickweave.Samples.Domain.Persons.Services;

namespace Brickweave.Samples.Domain.Persons.Commands
{
    public class RemovePersonPhoneHandler : ICommandHandler<RemovePersonPhone, PersonInfo>
    {
        private readonly IPersonRepository _personRepository;

        public RemovePersonPhoneHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<PersonInfo> HandleAsync(RemovePersonPhone command)
        {
            var person = await _personRepository.GetPersonAsync(command.Id);

            person.RemovePhone(command.PhoneId);

            await _personRepository.SavePersonAsync(person);

            return person.ToInfo();
        }
    }
}
