using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Models;
using Brickweave.Samples.Domain.Persons.Services;

namespace Brickweave.Samples.Domain.Persons.Queries
{
    public class ListPersonsHandler : IQueryHandler<ListPersons, IEnumerable<PersonInfo>>
    {
        private readonly IPersonInfoRepository _personInfoRepository;

        public ListPersonsHandler(IPersonInfoRepository personInfoRepository)
        {
            _personInfoRepository = personInfoRepository;
        }

        public async Task<IEnumerable<PersonInfo>> HandleAsync(ListPersons query)
        {
            return await _personInfoRepository.ListPeopleAsync();
        }
    }
}
