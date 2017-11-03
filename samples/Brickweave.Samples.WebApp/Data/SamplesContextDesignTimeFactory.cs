using System.IO;
using System.Reflection;
using Brickweave.Samples.Persistence.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Brickweave.Samples.WebApp.Data
{
    public class SamplesContextDesignTimeFactory : IDesignTimeDbContextFactory<SamplesContext>
    {
        public SamplesContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<Startup>()
                .Build();

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            var options = new DbContextOptionsBuilder<SamplesContext>()
                .UseSqlServer(configuration.GetConnectionString("eventstore"),
                    sql => sql.MigrationsAssembly(migrationsAssembly))
                .Options;
            
            return new SamplesContext(options);
        }
    }
}