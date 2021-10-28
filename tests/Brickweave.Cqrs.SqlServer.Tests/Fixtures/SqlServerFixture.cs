using System;
using System.Collections.Generic;
using System.Linq;
using Brickweave.Cqrs.SqlServer.Entities;
using Brickweave.Cqrs.SqlServer.Tests.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Brickweave.Cqrs.SqlServer.Tests.Fixtures
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

            var sqlCommandList = CreateList(
                new { Order = 1, SqlCommand = $"DELETE FROM [{CqrsDbContext.SCHEMA_NAME}].[{CommandQueueData.TABLE_NAME}]" });

            var sql = string.Join(Environment.NewLine, sqlCommandList
                .OrderBy(item => item.Order)
                .Select(item => item.SqlCommand)
                .ToArray());

            dbContext.Database.ExecuteSqlRaw(sql);
        }

        private static List<T> CreateList<T>(params T[] elements)
        {
            return new List<T>(elements);
        }
    }
}
