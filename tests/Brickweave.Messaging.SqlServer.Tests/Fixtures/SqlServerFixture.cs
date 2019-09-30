using Brickweave.Messaging.SqlServer.Entities;
using Brickweave.Messaging.SqlServer.Tests.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Brickweave.Messaging.SqlServer.Tests.Fixtures
{
    public class SqlServerFixture
    {
        public SqlServerFixture()
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<SqlServerFixture>()
                .AddEnvironmentVariables()
                .Build();

            var connectionString = config.GetConnectionString("brickweave_tests");

            DbContext = new MessageStoreDbContext(
                new DbContextOptionsBuilder<MessageStoreDbContext>().UseSqlServer(connectionString).Options);

            DbContext.Database.EnsureDeleted();
            DbContext.Database.EnsureCreated();
        }

        public MessageStoreDbContext DbContext { get; }

        public void ClearDatabase()
        {
            var sql = $"DELETE FROM [{MessageStoreDbContext.SCHEMA_NAME}].[{MessageData.TABLE_NAME}]";
            DbContext.Database.ExecuteSqlCommand(sql);
        }
    }
}
