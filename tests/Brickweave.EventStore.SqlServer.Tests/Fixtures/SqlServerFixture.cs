using System;
using Brickweave.EventStore.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Brickweave.EventStore.SqlServer.Tests.Fixtures
{
    public class SqlServerFixture
    {
        public SqlServerFixture()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddUserSecrets<SqlServerFixture>()
                .AddEnvironmentVariables()
                .Build();

            var connectionString = config.GetConnectionString("eventStore");

            DbContext = new EventStoreDbContext(
                new DbContextOptionsBuilder<EventStoreDbContext>().UseSqlServer(connectionString).Options);

            DbContext.Database.EnsureDeleted();
            DbContext.Database.EnsureCreated();
        }

        public EventStoreDbContext DbContext { get; }

        public void ClearDatabase()
        {
            var sql = $"DELETE FROM [{EventStoreDbContext.SCHEMA_NAME}].[{EventData.TABLE_NAME}]";
            DbContext.Database.ExecuteSqlCommand(sql);
        }
    }
}
