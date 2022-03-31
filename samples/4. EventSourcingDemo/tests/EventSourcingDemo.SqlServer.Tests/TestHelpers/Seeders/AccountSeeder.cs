using System.Linq;
using System.Threading.Tasks;
using Brickweave.Domain.Serialization;
using Brickweave.EventStore;
using Brickweave.EventStore.Factories;
using Brickweave.Serialization;
using EventSourcingDemo.Domain.Accounts.Models;
using EventSourcingDemo.Domain.Common.Serialization;
using EventSourcingDemo.SqlServer.Repositories;
using Newtonsoft.Json;

namespace EventSourcingDemo.SqlServer.Tests.TestHelpers.Seeders
{
    public class AccountSeeder
    {
        private readonly SqlServerAccountRepository _accountRepository;

        public AccountSeeder(EventSourcingDemoDbContext dbContext)
        {
            var assembly = typeof(Account).Assembly;
            var shortHandTypes = assembly.ExportedTypes
                .Where(t => typeof(IEvent).IsAssignableFrom(t))
                .ToArray();

            _accountRepository = new SqlServerAccountRepository(
                dbContext,
                new JsonDocumentSerializer(shortHandTypes,
                    converters: new JsonConverter[] { new IdConverter(), new NameConverter() }),
                new ReflectionConventionAggregateFactory());
        }

        public async Task SeedAsync(Account account)
        {
            await _accountRepository.SaveAccountAsync(account);
        }
    }
}
