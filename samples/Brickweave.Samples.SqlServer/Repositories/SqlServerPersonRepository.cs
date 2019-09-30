using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brickweave.EventStore;
using Brickweave.EventStore.Factories;
using Brickweave.EventStore.Serialization;
using Brickweave.EventStore.SqlServer;
using Brickweave.Samples.Domain.Persons.Extensions;
using Brickweave.Samples.Domain.Persons.Models;
using Brickweave.Samples.Domain.Persons.Services;
using Brickweave.Samples.SqlServer.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.Samples.SqlServer.Repositories
{
    public class SqlServerPersonRepository : AggregateRepository<Person>, IPersonRepository, IPersonInfoRepository, IPersonEventStreamRepository
    {
        private readonly SamplesDbContext _dbContext;
        
        public SqlServerPersonRepository(SamplesDbContext dbContext, IDocumentSerializer serializer,
            IAggregateFactory aggregateFactory) : base(serializer, aggregateFactory)
        {
            _dbContext = dbContext;
        }
        
        public async Task SavePersonAsync(Person person)
        {
            AddUncommittedEvents(_dbContext.Events, person, person.Id.Value);

            if (person.IsActive)
                await AddSnapshotAsync(person);
            else
                await RemoveSnapshotAsync(person);

            await _dbContext.SaveChangesAsync();

            person.ClearUncommittedEvents();
        }

        public async Task<Person> GetPersonAsync(PersonId id, DateTime? pointInTime = null)
        {
            return await CreateFromEventsAsync(_dbContext.Events, id.Value, pointInTime);
        }

        public async Task<PersonInfo> GetPersonInfoAsync(PersonId personId, DateTime? pointInTime = null)
        {
            if (pointInTime != null)
                return (await GetPersonAsync(personId, pointInTime)).ToInfo();

            return (await _dbContext.Persons
                .FirstOrDefaultAsync(p => p.Id == personId.Value))?
                .ToInfo();
        }

        public async Task<IEnumerable<PersonInfo>> ListPeopleAsync()
        {
            var data = await _dbContext.Persons.ToListAsync();

            return data.Select(p => p.ToInfo());
        }

        public async Task<LinkedList<IEvent>> GetPersonEventStreamJsonAsync(PersonId id)
        {
            return await GetEvents(_dbContext.Events, id.Value);
        }

        private async Task AddSnapshotAsync(Person person)
        {
            if (await PersonExistsAsync(person))
                _dbContext.Persons.Update(person.ToSnapshot());
            else
                _dbContext.Persons.Add(person.ToSnapshot());
        }

        private async Task RemoveSnapshotAsync(Person person)
        {
            var snapshot = await _dbContext.Persons
                .FirstOrDefaultAsync(p => p.Id == person.Id.Value);

            if (snapshot != null)
                _dbContext.Remove(snapshot);
        }

        private async Task<bool> PersonExistsAsync(Person person)
        {
            var data = await _dbContext.Persons.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == person.Id.Value);

            return data != null;
        }
    }
}
