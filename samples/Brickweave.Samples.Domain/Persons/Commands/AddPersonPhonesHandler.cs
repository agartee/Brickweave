using System.Threading.Tasks;
using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Extensions;
using Brickweave.Samples.Domain.Persons.Models;
using Brickweave.Samples.Domain.Persons.Services;
using Brickweave.Samples.Domain.Phones.Models;

namespace Brickweave.Samples.Domain.Persons.Commands
{
    public class AddPersonPhonesHandler : ICommandHandler<AddPersonPhones, PersonInfo>
    {
        private readonly IPersonRepository _personRepository;

        public AddPersonPhonesHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<PersonInfo> HandleAsync(AddPersonPhones command)
        {
            var person = await _personRepository.GetPersonAsync(command.Id);

            foreach(var number in command.PhoneNumbers)
                person.AddPhone(PhoneId.NewId(), number);

            await _personRepository.SavePersonAsync(person);

            return person.ToInfo();
        }
    }
}
