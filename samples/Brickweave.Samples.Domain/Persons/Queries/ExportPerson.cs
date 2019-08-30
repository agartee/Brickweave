using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Models;

namespace Brickweave.Samples.Domain.Persons.Queries
{
    public class ExportPerson : IQuery<string>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="personId"></param>
        public ExportPerson(PersonId personId)
        {
            PersonId = personId;
        }

        public PersonId PersonId { get; }
    }
}
