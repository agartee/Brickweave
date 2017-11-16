using System.Threading.Tasks;
using Brickweave.EventStore.Factories;
using Brickweave.EventStore.Serialization;
using Brickweave.EventStore.SqlServer;
using Brickweave.Samples.Domain.Persons.Models;
using Brickweave.Samples.Domain.Persons.Services;
using Brickweave.Samples.Persistence.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.Samples.Persistence.SqlServer.Repositories
{
    public class SqlServerPersonRepository : SqlServerAggregateRepository<Person>, IPersonRepository, IPersonInfoRepository
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
            await SaveSnapshotAsync(person);
        }

        public async Task<Person> GetPersonAsync(PersonId id)
        {
            return await TryFindAsync(id.Value);
        }

        public async Task<PersonInfo> GetPersonInfoAsync(PersonId personId)
        {
            var data = await _samplesContext.Persons
                .FirstOrDefaultAsync(p => p.Id == personId.Value);

            return data?.ToInfo();
        }

        private async Task SaveSnapshotAsync(Person person)
        {
            _samplesContext.Persons.Add(new PersonSnapshot
            {
                Id = person.Id.Value,
                FirstName = person.Name.FirstName,
                LastName = person.Name.LastName
            });

            await _samplesContext.SaveChangesAsync();
        }
    }
}
