using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brickweave.EventStore.SqlServer.Extensions;
using Brickweave.Samples.Domain.Persons.Models;
using Brickweave.Samples.Domain.Persons.Services;
using Brickweave.Samples.SqlServer.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.Samples.SqlServer.Repositories
{
    public class SqlServerPersonRepository : IPersonRepository, IPersonInfoRepository
    {
        private readonly SamplesDbContext _dbContext;
        
        public SqlServerPersonRepository(SamplesDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        
        public async Task SavePersonAsync(Person person)
        {
            _dbContext.Events.AddUncommittedEvents(person, person.Id.Value);
            await AddSnapshotAsync(person);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<Person> GetPersonAsync(PersonId id)
        {
            return await _dbContext.Events
                .CreateFromEventsAsync<Person>(id.Value);
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
