using System.Collections.Generic;
using System.Threading.Tasks;
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

    public class ListPersonsHandler : IQueryHandler<ListPersons, IEnumerable<PersonInfo>>
    {
        public async Task<IEnumerable<PersonInfo>> HandleAsync(ListPersons query)
        {
            return new List<PersonInfo>
            {
                new PersonInfo(PersonId.NewId(), new Name("Adam", "Gartee")),
                new PersonInfo(PersonId.NewId(), new Name("Tom", "Riddle"))
            };
        }
    }
}
