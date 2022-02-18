using System.Linq;
using System.Threading.Tasks;
using Brickweave.Domain.Serialization;
using Brickweave.EventStore;
using Brickweave.EventStore.Factories;
using Brickweave.Serialization;
using EventSourcingDemo.Domain.Accounts.Models;
using EventSourcingDemo.Domain.Common.Models;
using EventSourcingDemo.Domain.Common.Serialization;
using EventSourcingDemo.Domain.Companies.Models;
using EventSourcingDemo.Domain.People.Models;
using EventSourcingDemo.Domain.Tests.TestHelpers.Builders;
using EventSourcingDemo.SqlServer.Repositories;
using EventSourcingDemo.SqlServer.Tests.Fixtures;
using EventSourcingDemo.SqlServer.Tests.TestHelpers.Seeders;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Xunit;

namespace EventSourcingDemo.SqlServer.Tests.Repositories
{
    [Trait("Category", "Integration")]
    [Collection("SqlServerTestCollection")]
    public class SqlServerAccountRepositoryTests
    {
        private readonly SqlServerTestFixture _fixture;
        private readonly SqlServerAccountRepository _repository;

        public SqlServerAccountRepositoryTests(SqlServerTestFixture fixture)
        {
            _fixture = fixture;

            var assembly = typeof(Account).Assembly;
            var shortHandTypes = assembly.ExportedTypes
                .Where(t => typeof(IEvent).IsAssignableFrom(t))
                .ToArray();

            _repository = new SqlServerAccountRepository(
                fixture.CreateDbContext(),
                new JsonDocumentSerializer(shortHandTypes,
                    converters: new JsonConverter[] { new IdConverter(), new NameConverter() }),
                new ReflectionConventionAggregateFactory());
        }

        [Fact]
        public async Task SaveAccountAsync_WithNewAccountAndPersonAccountHolder_SavesEventsAndProjections()
        {
            await _fixture.ClearDataAsync();

            var accountId = AccountId.NewId();
            var accountName = new Name("Account 1");
            var accountHolderId = PersonId.NewId();

            await new PersonSeeder(_fixture.CreateDbContext())
                .SeedAsync(new PersonBuilder()
                    .WithId(accountHolderId)
                    .Build());

            var account = new Account(accountId, accountName, accountHolderId);

            await _repository.SaveAccountAsync(account);

            var data = await _fixture.CreateDbContext().PersonalAccounts
                .SingleAsync(r => r.Id == accountId.Value);

            data.Name.Should().Be(accountName.Value);
            data.AccountHolderId.Should().Be(accountHolderId.Value);
            data.Balance.Should().Be(0);
        }

        [Fact]
        public async Task SaveAccountAsync_WithNewAccountAndCompanyAccountHolder_SavesEventsAndProjections()
        {
            await _fixture.ClearDataAsync();

            var accountId = AccountId.NewId();
            var accountName = new Name("Account 1");
            var accountHolderId = CompanyId.NewId();

            await new CompanySeeder(_fixture.CreateDbContext())
                .SeedAsync(new CompanyBuilder()
                    .WithId(accountHolderId)
                    .Build());

            var account = new Account(accountId, accountName, accountHolderId);

            await _repository.SaveAccountAsync(account);

            var data = await _fixture.CreateDbContext().BusinessAccounts
                .SingleAsync(r => r.Id == accountId.Value);

            data.Name.Should().Be(accountName.Value);
            data.AccountHolderId.Should().Be(accountHolderId.Value);
            data.Balance.Should().Be(0);
        }

        [Fact]
        public async Task SaveAccountAsync_WhenExistingAccountIsClosed_SavesEventsDeletesProjections()
        {
            await _fixture.ClearDataAsync();

            var accountId = AccountId.NewId();
            var accountName = new Name("Account 1");
            var accountHolderId = PersonId.NewId();

            await new PersonSeeder(_fixture.CreateDbContext())
                .SeedAsync(new PersonBuilder()
                    .WithId(accountHolderId)
                    .Build());

            await new AccountSeeder(_fixture.CreateDbContext())
                .SeedAsync(new Account(accountId, accountName, accountHolderId));

            var account = await _repository.DemandAccountAsync(accountId);
            account.Close();

            await _repository.SaveAccountAsync(account);

            var data = await _fixture.CreateDbContext().PersonalAccounts
                .SingleOrDefaultAsync(r => r.Id == accountId.Value);

            data.Should().BeNull();
        }

        [Fact]
        public async Task DemandAccountAsync_WhenAccountExists_ReturnsAccount()
        {
            await _fixture.ClearDataAsync();

            var accountId = AccountId.NewId();
            var accountName = new Name("Account 1");
            var accountHolderId = PersonId.NewId();

            await new PersonSeeder(_fixture.CreateDbContext())
                .SeedAsync(new PersonBuilder()
                    .WithId(accountHolderId)
                    .Build());

            await new AccountSeeder(_fixture.CreateDbContext())
                .SeedAsync(new Account(accountId, accountName, accountHolderId));

            var result = await _repository.DemandAccountAsync(accountId);

            result.Id.Should().Be(accountId);
            result.Name.Should().Be(accountName);
            result.AccountHolderId.Should().Be(accountHolderId);
            result.Balance.Should().Be(0);
        }
    }
}
