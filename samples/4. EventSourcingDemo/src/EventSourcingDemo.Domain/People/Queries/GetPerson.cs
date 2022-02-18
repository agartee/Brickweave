using Brickweave.Cqrs;
using EventSourcingDemo.Domain.People.Models;

namespace EventSourcingDemo.Domain.People.Queries
{
    public class GetPerson : IQuery<PersonInfo>
    {
        public GetPerson(PersonId personId)
        {
            PersonId = personId ?? throw new ArgumentNullException(nameof(personId));
        }

        public PersonId PersonId { get; }
    }
}
