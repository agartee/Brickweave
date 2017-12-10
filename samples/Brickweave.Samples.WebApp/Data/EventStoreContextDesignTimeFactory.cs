using System.IO;
using System.Reflection;
using Brickweave.EventStore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Brickweave.Samples.WebApp.Data
{
    public class EventStoreContextDesignTimeFactory : IDesignTimeDbContextFactory<EventStoreContext>
    {
        public EventStoreContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<Startup>()
                .Build();

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            var options = new DbContextOptionsBuilder<EventStoreContext>()
                .UseSqlServer(configuration.GetConnectionString("eventstore"),
                    sql => sql.MigrationsAssembly(migrationsAssembly))
                .Options;

            return new EventStoreContext(options);
        }
    }
}