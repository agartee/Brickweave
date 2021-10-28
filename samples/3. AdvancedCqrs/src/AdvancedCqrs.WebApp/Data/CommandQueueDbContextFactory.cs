using System.IO;
using System.Reflection;
using AdvancedCqrs.CommandQueue.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AdvancedCqrs.WebApp.Data
{
    public class CommandQueueDbContextFactory : IDesignTimeDbContextFactory<CommandQueueDbContext>
    {
        public static void Migrate()
        {
            new CommandQueueDbContextFactory().CreateDbContext(new string[] { })
                .Database.Migrate();
        }

        public CommandQueueDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<Startup>()
                .Build();

            var migrationsAssembly = typeof(CommandQueueDbContext).GetTypeInfo().Assembly.GetName().Name;

            var options = new DbContextOptionsBuilder<CommandQueueDbContext>()
                .UseSqlServer(configuration.GetConnectionString("demo"),
                    sql => sql.CommandTimeout(525600 * 60).MigrationsAssembly(migrationsAssembly))
                .Options;

            return new CommandQueueDbContext(options);
        }
    }
}
