using System.Threading.Tasks;
using BasicCqrs.Domain.People.Services;
using Brickweave.Cqrs;

namespace BasicCqrs.Domain.People.Commands
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
            await _personRepository.DeletePerson(command.Id);
        }
    }
}
