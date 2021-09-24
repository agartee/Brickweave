using BasicCqrs.Domain.People.Models;
using Brickweave.Cqrs;

namespace BasicCqrs.Domain.People.Queries
{
    public class GetPerson : IQuery<Person>
    {
        /// <summary>
        /// Gets an existing person.
        /// </summary>
        /// <param name="id">The unique ID of an existing person.</param>
        public GetPerson(PersonId id)
        {
            Id = id;
        }

        public PersonId Id { get; }
    }
}
