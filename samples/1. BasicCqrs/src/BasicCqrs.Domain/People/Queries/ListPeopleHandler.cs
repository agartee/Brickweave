using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasicCqrs.Domain.People.Models;
using BasicCqrs.Domain.People.Services;
using Brickweave.Cqrs;

namespace BasicCqrs.Domain.People.Queries
{
    public class ListPeopleHandler : IQueryHandler<ListPeople, IEnumerable<Person>>
    {
        private readonly IPersonRepository personRepository;

        public ListPeopleHandler(IPersonRepository personRepository)
        {
            this.personRepository = personRepository;
        }

        public async Task<IEnumerable<Person>> HandleAsync(ListPeople query)
        {
            var people = await personRepository.ListPeopleAsync(
                new PersonSearchCriteria(query.NameLike));

            return people
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .ToList();
        }
    }
}
