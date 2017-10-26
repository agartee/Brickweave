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
        public SqlServerPersonRepository(EventStoreContext dbContext, IDocumentSerializer serializer, 
            IAggregateFactory aggregateFactory) : base(dbContext, serializer, aggregateFactory)
        {
        }


        public async Task SavePersonAsync(Person person)
        {
            await SaveUncommittedEventsAsync(person, person.Id.Value);
        }

        public async Task<Person> GetPersonAsync(PersonId id)
        {
            return await TryFindAsync(id.Value);
        }
    }
}
