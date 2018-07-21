using System.Collections.Generic;
using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Models;

namespace Brickweave.Samples.Domain.Persons.Queries
{
    public class ListPersons : IQuery<IEnumerable<PersonInfo>>
    {
        /// <summary>
        /// List people
        /// </summary>
        /// <param name="attributes">person attributes</param>
        public ListPersons(IDictionary<string, IEnumerable<string>> attributes)
        {
            Attributes = attributes;
        }

        public IDictionary<string, IEnumerable<string>> Attributes { get; }
    }
}
