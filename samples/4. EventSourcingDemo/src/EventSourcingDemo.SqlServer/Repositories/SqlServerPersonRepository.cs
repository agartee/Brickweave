using EventSourcingDemo.Domain.People.Models;
using EventSourcingDemo.Domain.People.Services;
using EventSourcingDemo.SqlServer.Entities;
using EventSourcingDemo.SqlServer.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EventSourcingDemo.SqlServer.Repositories
{
    public class SqlServerPersonRepository : IPersonRepository
    {
        private readonly EventSourcingDemoDbContext _dbContext;

        public SqlServerPersonRepository(EventSourcingDemoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Person> DemandPersonAsync(PersonId id)
        {
            var data = await _dbContext.People
                .SingleAsync(p => p.Id == id.Value);

            return data.ToPerson();
        }

        public async Task SavePersonAsync(Person person)
        {
            var data = await _dbContext.People
                .SingleOrDefaultAsync(p => p.Id == person.Id.Value);

            if (data == null)
                data = CreateData(person);
            else
                UpdateData(person, data);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Person>> ListPeopleAsync()
        {
            var data = await _dbContext.People
                .ToListAsync();

            return data
                .Select(p => p.ToPerson())
                .ToList();
        }

        public async Task DeletePersonAsync(PersonId id)
        {
            var data = await _dbContext.People
                .SingleOrDefaultAsync(p => p.Id == id.Value);

            if (data == null)
                return;

            _dbContext.People.Remove(data);
            await _dbContext.SaveChangesAsync();
        }

        private PersonData CreateData(Person person)
        {
            var data = new PersonData
            {
                Id = person.Id.Value,
                Name = person.Name.Value,
            };
            _dbContext.People.Add(data);

            return data;
        }

        private static void UpdateData(Person person, PersonData data)
        {
            data.Name = person.Name.Value;
        }
    }
}
