using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Models;

namespace Brickweave.Samples.Domain.Persons.Queries
{
    public class GetPerson : IQuery<PersonInfo>
    {
        /// <summary>
        /// Get a person
        /// </summary>
        /// <param name="id"></param>
        public GetPerson(PersonId id)
        {
            Id = id;
        }

        public PersonId Id { get; }
    }
}
