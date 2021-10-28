using System.IO;
using System.Reflection;
using AdvancedCqrs.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AdvancedCqrs.WebApp.Data
{
    public class AdvancedCqrsDbContextFactory : IDesignTimeDbContextFactory<AdvancedCqrsDbContext>
    {
        public static void Migrate()
        {
            new AdvancedCqrsDbContextFactory().CreateDbContext(new string[] { })
                .Database.Migrate();
        }

        public AdvancedCqrsDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<Startup>()
                .Build();

            var migrationsAssembly = typeof(AdvancedCqrsDbContext).GetTypeInfo().Assembly.GetName().Name;

            var options = new DbContextOptionsBuilder<AdvancedCqrsDbContext>()
                .UseSqlServer(configuration.GetConnectionString("demo"),
                    sql => sql.CommandTimeout(525600 * 60).MigrationsAssembly(migrationsAssembly))
                .Options;

            return new AdvancedCqrsDbContext(options);
        }
    }
}
