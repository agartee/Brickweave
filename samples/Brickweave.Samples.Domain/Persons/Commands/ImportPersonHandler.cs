using System.Linq;
using System.Threading.Tasks;
using Brickweave.Cqrs;
using Brickweave.EventStore;
using Brickweave.EventStore.Serialization;
using Brickweave.Samples.Domain.Persons.Extensions;
using Brickweave.Samples.Domain.Persons.Models;
using Brickweave.Samples.Domain.Persons.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Brickweave.Samples.Domain.Persons.Commands
{
    public class ImportPersonHandler : ICommandHandler<ImportPerson, PersonInfo>
    {
        private readonly IPersonRepository _personRepository;
        private readonly IDocumentSerializer _serializer;

        public ImportPersonHandler(IPersonRepository personRepository, IDocumentSerializer serializer)
        {
            _personRepository = personRepository;
            _serializer = serializer;
        }

        public async Task<PersonInfo> HandleAsync(ImportPerson command)
        {
            var results = JsonConvert.DeserializeObject<JArray>(command.Json);

            var events = results
                .Select(t => _serializer.DeserializeObject<IEvent>(t.ToString()))
                .ToList();

            var person = Person.CreateFromEvents(events);

            await _personRepository.SavePersonAsync(person);

            return person.ToInfo();
        }
    }
}
