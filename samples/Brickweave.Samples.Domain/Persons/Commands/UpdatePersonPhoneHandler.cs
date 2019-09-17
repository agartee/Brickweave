using System.Linq;
using System.Threading.Tasks;
using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Extensions;
using Brickweave.Samples.Domain.Persons.Models;
using Brickweave.Samples.Domain.Persons.Services;

namespace Brickweave.Samples.Domain.Persons.Commands
{
    public class UpdatePersonPhoneHandler : ICommandHandler<UpdatePersonPhone, PersonInfo>
    {
        private readonly IPersonRepository _personRepository;

        public UpdatePersonPhoneHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<PersonInfo> HandleAsync(UpdatePersonPhone command)
        {
            var person = await _personRepository.GetPersonAsync(command.PersonId);

            var phone = person.Phones.First(p => p.Id.Equals(command.PhoneId));

            if (command.PhoneType != null)
                phone.UpdateType(command.PhoneType.Value);
            if (command.Number != null)
                phone.UpdateNumber(command.Number);

            await _personRepository.SavePersonAsync(person);

            return person.ToInfo();
        }
    }
}
