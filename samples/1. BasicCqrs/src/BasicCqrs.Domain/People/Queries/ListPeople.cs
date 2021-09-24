using System.Collections.Generic;
using BasicCqrs.Domain.People.Models;
using Brickweave.Cqrs;

namespace BasicCqrs.Domain.People.Queries
{
    public class ListPeople : IQuery<IEnumerable<Person>>
    {
        /// <summary>
        /// Get a list of existing people.
        /// </summary>
        /// <param name="nameLike">Name criteria for a partial match of either first or last name.</param>
        public ListPeople(string nameLike = null)
        {
            NameLike = nameLike;
        }

        public string NameLike { get; }
    }
}
