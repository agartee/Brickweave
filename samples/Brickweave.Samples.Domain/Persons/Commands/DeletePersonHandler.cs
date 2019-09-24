using System.Threading.Tasks;
using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Services;

namespace Brickweave.Samples.Domain.Persons.Commands
{
    public class DeletePersonHandler : ICommandHandler<DeletePerson>
    {
        private readonly IPersonRepository _personRepository;

        public DeletePersonHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task HandleAsync(DeletePerson command)
        {
            var person = await _personRepository.GetPersonAsync(command.PersonId);

            person.Delete();

            await _personRepository.SavePersonAsync(person);
        }
    }
}
