using System;
using System.Linq;
using System.Threading.Tasks;
using Brickweave.Samples.Domain.Persons.Models;
using Brickweave.Samples.Domain.Persons.Services;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.Samples.Persistence.SqlServer.Repositories
{
    public class SqlServerPersonRepository : IPersonRepository
    {
        private readonly SampleDbContext _dbContext;

        public SqlServerPersonRepository(SampleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveAsync(Person person)
        {
            _dbContext.Persons.Add(person);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Person> GetPersonAsync(Guid id)
        {
            return await _dbContext.Persons
                .Where(p => p.Id == id)
                .SingleOrDefaultAsync();
        }
    }
}
