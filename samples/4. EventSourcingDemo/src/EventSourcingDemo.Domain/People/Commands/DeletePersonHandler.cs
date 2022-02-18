using Brickweave.Cqrs;
using EventSourcingDemo.Domain.People.Services;

namespace EventSourcingDemo.Domain.People.Commands
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
            await _personRepository.DeletePersonAsync(command.PersonId);
        }
    }
}
