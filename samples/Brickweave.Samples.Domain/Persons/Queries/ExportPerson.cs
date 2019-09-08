using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Models;

namespace Brickweave.Samples.Domain.Persons.Queries
{
    public class ExportPerson : IQuery<string>
    {
        /// <summary>
        /// Export a single existing person's event stream.
        /// </summary>
        /// <param name="personId"></param>
        public ExportPerson(PersonId personId)
        {
            PersonId = personId;
        }

        public PersonId PersonId { get; }
    }
}
