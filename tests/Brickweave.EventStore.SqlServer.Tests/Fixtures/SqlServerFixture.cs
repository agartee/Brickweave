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
                .AddJsonFile("appsettings.local.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = config.GetConnectionString("eventStore");

            DbContext = new EventStoreContext(
                new DbContextOptionsBuilder().UseSqlServer(connectionString).Options);

            DbContext.Database.EnsureCreated();
        }

        public EventStoreContext DbContext { get; }

        public void ClearDatabase()
        {
            var sql = $"DELETE FROM [{DbContext.Schema}].[{EventData.TABLE_NAME}]";
            DbContext.Database.ExecuteSqlCommand(sql);
        }
    }
}
