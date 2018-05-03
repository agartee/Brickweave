using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brickweave.EventStore.Factories;
using Brickweave.EventStore.Serialization;
using Brickweave.EventStore.SqlServer;
using Brickweave.Samples.Domain.Persons.Models;
using Brickweave.Samples.Domain.Persons.Services;
using Brickweave.Samples.SqlServer.Entities;
using Brickweave.Samples.SqlServer.Extensions;
using Brickweave.Samples.SqlServer.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.Samples.SqlServer.Repositories
{
    public class SqlServerPersonRepository : SqlServerAggregateRepository<Person, SamplesDbContext>, 
        IPersonRepository, IPersonInfoRepository
    {
        private readonly SamplesDbContext _dbContext;

        public SqlServerPersonRepository(SamplesDbContext dbContext, SamplesDbContext samplesContext, 
            IDocumentSerializer serializer, IAggregateFactory aggregateFactory) 
            : base(dbContext, serializer, aggregateFactory)
        {
            _dbContext = samplesContext;
        }
        
        public async Task SavePersonAsync(Person person)
        {
            await SaveUncommittedEventsAsync(person, person.Id.Value,
                async () => await AddSnapshotAsync(person));
        }

        public async Task<Person> GetPersonAsync(PersonId id)
        {
            return await TryFindAsync(id.Value);
        }

        public async Task<PersonInfo> GetPersonInfoAsync(PersonId personId)
        {
            var data = await _dbContext.Persons
                .FirstOrDefaultAsync(p => p.Id == personId.Value);

            return data?.ToInfo();
        }

        public async Task<IEnumerable<PersonInfo>> ListPeopleAsync()
        {
            var data = await _dbContext.Persons.ToListAsync();

            return data.Select(p => p.ToInfo());
        }

        private async Task AddSnapshotAsync(Person person)
        {
            if (await PersonExistsAsync(person))
                _dbContext.Persons.Update(person.ToSnapshot());
            else
                _dbContext.Persons.Add(person.ToSnapshot());
        }

        private async Task<bool> PersonExistsAsync(Person person)
        {
            var data = await _dbContext.Persons.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == person.Id.Value);

            return data != null;
        }
    }
}
