using System.Threading.Tasks;
using Brickweave.EventStore.Factories;
using Brickweave.EventStore.Serialization;
using Brickweave.EventStore.SqlServer;
using Brickweave.Samples.Domain.Persons.Models;
using Brickweave.Samples.Domain.Persons.Services;

namespace Brickweave.Samples.Persistence.SqlServer.Repositories
{
    public class SqlServerPersonRepository : SqlServerAggregateRepository<Person>, IPersonRepository
    {
        private readonly SamplesContext _samplesContext;

        public SqlServerPersonRepository(EventStoreContext dbContext, SamplesContext samplesContext, 
            IDocumentSerializer serializer, IAggregateFactory aggregateFactory) 
            : base(dbContext, serializer, aggregateFactory)
        {
            _samplesContext = samplesContext;
        }
        
        public async Task SavePersonAsync(Person person)
        {
            await SaveUncommittedEventsAsync(person, person.Id.Value);

            _samplesContext.Persons.Add(new PersonData
            {
                Id = person.Id.Value,
                FirstName = person.Name.FirstName,
                LastName = person.Name.LastName
            });
            
            await _samplesContext.SaveChangesAsync();
        }

        public async Task<Person> GetPersonAsync(PersonId id)
        {
            return await TryFindAsync(id.Value);
        }
    }
}
