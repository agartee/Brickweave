using System.Threading.Tasks;
using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Services;
using Brickweave.Serialization;

namespace Brickweave.Samples.Domain.Persons.Queries
{
    public class ExportPersonHandler : IQueryHandler<ExportPerson, string>
    {
        private readonly IPersonEventStreamRepository _personEventStreamRepository;
        private readonly IDocumentSerializer _serializer;

        public ExportPersonHandler(IPersonEventStreamRepository personEventStreamRepository, IDocumentSerializer serializer)
        {
            _personEventStreamRepository = personEventStreamRepository;
            _serializer = serializer;
        }

        public async Task<string> HandleAsync(ExportPerson query)
        {
            var events = await _personEventStreamRepository.GetPersonEventStreamJsonAsync(query.PersonId);

            return _serializer.SerializeObject(events);
        }
    }
}
