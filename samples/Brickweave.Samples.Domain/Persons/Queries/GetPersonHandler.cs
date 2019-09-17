using System.Threading.Tasks;
using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Models;
using Brickweave.Samples.Domain.Persons.Services;

namespace Brickweave.Samples.Domain.Persons.Queries
{
    public class GetPersonHandler : IQueryHandler<GetPerson, PersonInfo>
    {
        private readonly IPersonInfoRepository _personInfoRepository;

        public GetPersonHandler(IPersonInfoRepository personInfoRepository)
        {
            _personInfoRepository = personInfoRepository;
        }

        public async Task<PersonInfo> HandleAsync(GetPerson query)
        {
            return await _personInfoRepository.GetPersonInfoAsync(query.PersonId, query.PointInTime);
        }
    }
}
