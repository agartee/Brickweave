using System.Threading.Tasks;
using EventSourcingDemo.Domain.Companies.Models;
using EventSourcingDemo.SqlServer.Repositories;

namespace EventSourcingDemo.SqlServer.Tests.TestHelpers.Seeders
{
    public class CompanySeeder
    {
        private readonly SqlServerCompanyRepository _rulesetRepository;

        public CompanySeeder(EventSourcingDemoDbContext dbContext)
        {
            _rulesetRepository = new SqlServerCompanyRepository(
                dbContext);
        }

        public async Task SeedAsync(Company person)
        {
            await _rulesetRepository.SaveCompanyAsync(person);
        }
    }
}
