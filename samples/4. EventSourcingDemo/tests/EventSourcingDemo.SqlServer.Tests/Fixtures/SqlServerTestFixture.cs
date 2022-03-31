using System;
using System.Linq;
using System.Threading.Tasks;
using EventSourcingDemo.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EventSourcingDemo.SqlServer.Tests.Fixtures
{
    public class SqlServerTestFixture
    {
        private readonly IConfiguration _config;

        public SqlServerTestFixture()
        {
            _config = new ConfigurationBuilder()
                .AddUserSecrets<SqlServerTestFixture>()
                .AddEnvironmentVariables()
                .Build();

            var configurationDbContext = CreateDbContext();

            configurationDbContext.Database.EnsureDeleted();
            configurationDbContext.Database.EnsureCreated();
        }

        public EventSourcingDemoDbContext CreateDbContext()
        {
            return new EventSourcingDemoDbContext(new DbContextOptionsBuilder<EventSourcingDemoDbContext>()
                .UseSqlServer(_config.GetConnectionString("demo")).Options);
        }

        public async Task ClearDataAsync()
        {
            var dbContext = CreateDbContext();

            var sqlCommandList = new[]
            {
                new { Order = 2, SqlCommand = $"DELETE FROM [{EventSourcingDemoDbContext.SCHEMA_NAME}].[{PersonData.TABLE_NAME}]" },
                new { Order = 2, SqlCommand = $"DELETE FROM [{EventSourcingDemoDbContext.SCHEMA_NAME}].[{CompanyData.TABLE_NAME}]" },
                new { Order = 1, SqlCommand = $"DELETE FROM [{EventSourcingDemoDbContext.SCHEMA_NAME}].[{BusinessAccountData.TABLE_NAME}]" },
                new { Order = 1, SqlCommand = $"DELETE FROM [{EventSourcingDemoDbContext.SCHEMA_NAME}].[{PersonalAccountData.TABLE_NAME}]" }
            };

            var sql = string.Join(Environment.NewLine, sqlCommandList
                .OrderBy(item => item.Order)
                .Select(item => item.SqlCommand)
                .ToArray());

            await dbContext.Database.ExecuteSqlRawAsync(sql);
        }
    }
}
