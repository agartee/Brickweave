using System.Collections.Generic;
using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Models;

namespace Brickweave.Samples.Domain.Persons.Queries
{
    public class ListPersons : IQuery<IEnumerable<PersonInfo>>
    {
        /// <summary>
        /// List existing people.
        /// </summary>
        /// <param name="attributes">person attributes</param>
        public ListPersons(IDictionary<string, IEnumerable<object>> attributes = null)
        {
            Attributes = attributes ?? new Dictionary<string, IEnumerable<object>>();
        }

        public IDictionary<string, IEnumerable<object>> Attributes { get; }
    }
}
