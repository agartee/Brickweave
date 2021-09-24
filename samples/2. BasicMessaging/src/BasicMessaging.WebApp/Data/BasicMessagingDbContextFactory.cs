using System.IO;
using System.Reflection;
using BasicMessaging.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BasicMessaging.WebApp.Data
{
    public class BasicMessagingDbContextFactory : IDesignTimeDbContextFactory<BasicMessagingDbContext>
    {
        public static void Migrate()
        {
            new BasicMessagingDbContextFactory().CreateDbContext(new string[] { })
                .Database.Migrate();
        }

        public BasicMessagingDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<Startup>()
                .Build();

            var migrationsAssembly = typeof(BasicMessagingDbContext).GetTypeInfo().Assembly.GetName().Name;

            var options = new DbContextOptionsBuilder<BasicMessagingDbContext>()
                .UseSqlServer(configuration.GetConnectionString("demo"),
                    sql => sql.CommandTimeout(525600 * 60).MigrationsAssembly(migrationsAssembly))
                .Options;

            return new BasicMessagingDbContext(options);
        }
    }
}
