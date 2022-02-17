using System.Threading.Tasks;
using EventSourcingDemo.Domain.People.Models;
using EventSourcingDemo.SqlServer.Repositories;

namespace EventSourcingDemo.SqlServer.Tests.TestHelpers.Seeders
{
    public class PersonSeeder
    {
        private readonly SqlServerPersonRepository _rulesetRepository;

        public PersonSeeder(EventSourcingDemoDbContext dbContext)
        {
            _rulesetRepository = new SqlServerPersonRepository(
                dbContext);
        }

        public async Task SeedAsync(Person person)
        {
            await _rulesetRepository.SavePersonAsync(person);
        }
    }
}
