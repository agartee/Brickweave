using System.Threading.Tasks;
using BasicCqrs.Domain.People.Models;
using BasicCqrs.Domain.People.Services;
using Brickweave.Cqrs;

namespace BasicCqrs.Domain.People.Queries
{
    public class GetPersonHandler : IQueryHandler<GetPerson, Person>
    {
        private readonly IPersonRepository personRepository;

        public GetPersonHandler(IPersonRepository personRepository)
        {
            this.personRepository = personRepository;
        }

        public async Task<Person> HandleAsync(GetPerson query)
        {
            var person = await personRepository.GetPersonAsync(query.Id);

            return person;
        }
    }
}
