using System.Threading.Tasks;
using Brickweave.EventStore.SqlServer.Entities;
using Brickweave.EventStore.SqlServer.Tests.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Brickweave.EventStore.SqlServer.Tests.Fixtures
{
    public class SqlServerFixture
    {
        private readonly IConfiguration _config;

        public SqlServerFixture()
        {
            _config = new ConfigurationBuilder()
                .AddUserSecrets<SqlServerFixture>()
                .AddEnvironmentVariables()
                .Build();

            var configurationDbContext = CreateDbContext();

            configurationDbContext.Database.EnsureDeleted();
            configurationDbContext.Database.EnsureCreated();
        }

        public EventStoreDbContext CreateDbContext()
        {
            return new EventStoreDbContext(new DbContextOptionsBuilder<EventStoreDbContext>()
                .UseSqlServer(_config.GetConnectionString("brickweave-tests")).Options);
        }

        public void ClearData()
        {
            var dbContext = CreateDbContext();

            var sql = $"DELETE FROM [{EventStoreDbContext.SCHEMA_NAME}].[{EventData.TABLE_NAME}]";

            dbContext.Database.ExecuteSqlRaw(sql);
        }
    }
}
