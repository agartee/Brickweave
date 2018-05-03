using System.Collections.Generic;
using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Models;

namespace Brickweave.Samples.Domain.Persons.Queries
{
    public class ListPersons : IQuery<IEnumerable<PersonInfo>>
    {
        /// <summary>
        /// List existing people
        /// </summary>
        public ListPersons()
        {
        }
    }
}
