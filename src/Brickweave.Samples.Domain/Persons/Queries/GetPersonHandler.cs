using System.Threading.Tasks;
using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Models;
using Brickweave.Samples.Domain.Persons.Services;

namespace Brickweave.Samples.Domain.Persons.Queries
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
            var person = await _personRepository.GetPersonAsync(query.Id);
            return person.ToInfo();
        }
    }
}