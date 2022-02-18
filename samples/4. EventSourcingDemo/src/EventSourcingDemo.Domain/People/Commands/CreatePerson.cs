using Brickweave.Cqrs;
using EventSourcingDemo.Domain.Common.Models;
using EventSourcingDemo.Domain.People.Models;

namespace EventSourcingDemo.Domain.People.Commands
{
    public class CreatePerson : ICommand<PersonInfo>
    {
        public CreatePerson(Name name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public Name Name { get; }
    }
}
