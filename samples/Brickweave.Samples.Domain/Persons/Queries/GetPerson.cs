using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Models;

namespace Brickweave.Samples.Domain.Persons.Queries
{
    public class GetPerson : IQuery<PersonInfo>
    {
        public GetPerson(PersonId id)
        {
            Id = id;
        }

        public PersonId Id { get; }
    }
}
