using System.Collections.Generic;
using System.Threading.Tasks;
using Brickweave.Cqrs;
using Brickweave.EventStore;
using Brickweave.EventStore.Serialization;
using Brickweave.Samples.Domain.Persons.Services;

namespace Brickweave.Samples.Domain.Persons.Queries
{
    public class ExportPeopleHandler : IQueryHandler<ExportPeople, string>
    {
        private readonly IPersonInfoRepository _personInfoRepository;
        private readonly IPersonEventStreamRepository _personEventStreamRepository;
        private readonly IDocumentSerializer _serializer;

        public ExportPeopleHandler(IPersonInfoRepository personInfoRepository, 
            IPersonEventStreamRepository personEventStreamRepository, 
            IDocumentSerializer serializer)
        {
            _personInfoRepository = personInfoRepository;
            _personEventStreamRepository = personEventStreamRepository;
            _serializer = serializer;
        }

        public async Task<string> HandleAsync(ExportPeople query)
        {
            var people = await _personInfoRepository.ListPeopleAsync();

            var allEvents = new List<IEnumerable<IEvent>>();
            foreach(var person in people)
            {
                var events = await _personEventStreamRepository.GetPersonEventStreamJsonAsync(person.Id);
                allEvents.Add(events);
            }

            return _serializer.SerializeObject(allEvents);
        }
    }
}
