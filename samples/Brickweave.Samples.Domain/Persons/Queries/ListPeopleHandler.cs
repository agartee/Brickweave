using System.Collections.Generic;
using System.Threading.Tasks;
using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Models;
using Brickweave.Samples.Domain.Persons.Services;

namespace Brickweave.Samples.Domain.Persons.Queries
{
    public class ListPeopleHandler : IQueryHandler<ListPeople, IEnumerable<PersonInfo>>
    {
        private readonly IPersonInfoRepository _personInfoRepository;

        public ListPeopleHandler(IPersonInfoRepository personInfoRepository)
        {
            _personInfoRepository = personInfoRepository;
        }

        public async Task<IEnumerable<PersonInfo>> HandleAsync(ListPeople query)
        {
            return await _personInfoRepository.ListPeopleAsync();
        }
    }
}
