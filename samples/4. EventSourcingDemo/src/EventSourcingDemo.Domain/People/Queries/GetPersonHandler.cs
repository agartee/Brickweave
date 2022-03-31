using Brickweave.Cqrs;
using EventSourcingDemo.Domain.People.Extensions;
using EventSourcingDemo.Domain.People.Models;
using EventSourcingDemo.Domain.People.Services;

namespace EventSourcingDemo.Domain.People.Queries
{
    public class GetPersonHandler : IQueryHandler<GetPerson, PersonInfo>
    {
        private readonly IPersonRepository _personRepository;

        public GetPersonHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<PersonInfo> HandleAsync(GetPerson query)
        {
            var result = await _personRepository.DemandPersonAsync(query.PersonId);

            return result.ToPersonInfo();
        }
    }
}
