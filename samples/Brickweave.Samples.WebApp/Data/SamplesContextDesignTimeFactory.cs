using System.IO;
using System.Reflection;
using Brickweave.Samples.Persistence.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Brickweave.Samples.WebApp.Data
{
    public class SamplesContextDesignTimeFactory : IDesignTimeDbContextFactory<SamplesDbContext>
    {
        public SamplesDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<Startup>()
                .Build();

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            var options = new DbContextOptionsBuilder<SamplesDbContext>()
                .UseSqlServer(configuration.GetConnectionString("brickweave_samples"),
                    sql => sql.MigrationsAssembly(migrationsAssembly))
                .Options;

            return new SamplesDbContext(options);
        }
    }
}