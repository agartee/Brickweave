using Brickweave.Cqrs;
using EventSourcingDemo.Domain.People.Models;
using EventSourcingDemo.Domain.People.Services;

namespace EventSourcingDemo.Domain.People.Queries
{
    public class ListPeopleHandler : IQueryHandler<ListPeople, IEnumerable<PersonInfo>>
    {
        private readonly IPersonRepository _personRepository;

        public ListPeopleHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<IEnumerable<PersonInfo>> HandleAsync(ListPeople query)
        {
            var results = await _personRepository.ListPeopleAsync();

            return results
                .OrderBy(p => p.Name)
                .ToList();
        }
    }
}
