using System.IO;
using System.Reflection;
using BasicMessaging.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BasicMessaging.WebApp.Data
{
    public class MessagingDemoDbContextFactory : IDesignTimeDbContextFactory<MessagingDemoDbContext>
    {
        public static void Migrate()
        {
            new MessagingDemoDbContextFactory().CreateDbContext(new string[] { })
                .Database.Migrate();
        }

        public MessagingDemoDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<Startup>()
                .Build();

            var migrationsAssembly = typeof(MessagingDemoDbContext).GetTypeInfo().Assembly.GetName().Name;

            var options = new DbContextOptionsBuilder<MessagingDemoDbContext>()
                .UseSqlServer(configuration.GetConnectionString("demo"),
                    sql => sql.CommandTimeout(525600 * 60).MigrationsAssembly(migrationsAssembly))
                .Options;

            return new MessagingDemoDbContext(options);
        }
    }
}
