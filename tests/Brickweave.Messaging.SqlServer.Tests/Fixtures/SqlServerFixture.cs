using System;
using Brickweave.Messaging.SqlServer;
using Brickweave.Messaging.SqlServer.Entities;
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

            var connectionString = config.GetConnectionString("brickweave_tests");

            DbContext = new MessagingDbContext(
                new DbContextOptionsBuilder<MessagingDbContext>().UseSqlServer(connectionString).Options);

            DbContext.Database.EnsureDeleted();
            DbContext.Database.EnsureCreated();
        }

        public MessagingDbContext DbContext { get; }

        public void ClearDatabase()
        {
            var sql = $"DELETE FROM [{MessagingDbContext.SCHEMA_NAME}].[{MessageFailureData.TABLE_NAME}]";
            DbContext.Database.ExecuteSqlCommand(sql);
        }
    }
}
