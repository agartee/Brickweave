using Brickweave.Cqrs.Cli.SqlServer.Entities;
using Brickweave.Cqrs.Cli.SqlServer.Tests.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Brickweave.Cqrs.Cli.SqlServer.Tests.Fixtures
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

        public CqrsDbContext CreateDbContext()
        {
            return new CqrsDbContext(new DbContextOptionsBuilder<CqrsDbContext>()
                .UseSqlServer(_config.GetConnectionString("brickweave-tests")).Options);
        }

        public void ClearData()
        {
            var dbContext = CreateDbContext();

            var sql = $"DELETE FROM [{CqrsDbContext.SCHEMA_NAME}].[{ExecutionStatusData.TABLE_NAME}]";
            
            dbContext.Database.ExecuteSqlRaw(sql);
        }
    }
}
