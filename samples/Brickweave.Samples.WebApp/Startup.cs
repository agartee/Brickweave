using System;
using System.Linq;
using Brickweave.Cqrs.DependencyInjection;
using Brickweave.Samples.Domain.Persons.Services;
using Brickweave.Samples.Persistence.SqlServer;
using Brickweave.Samples.Persistence.SqlServer.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Brickweave.Samples.WebApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            
            var dbConfig = new SampleDbConfiguration { ConnectionString = "server=localhost\\sqlexpress;database=samples;integrated security=true;" };
            var dbContext = new SampleDbContext(dbConfig);
            dbContext.Database.EnsureCreated();

            var domainAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName.StartsWith("Brickweave"))
                .Where(a => a.FullName.Contains("Domain"))
                .ToArray();

            var domainServices = new ServiceCollection()
                .AddCommandExecutor()
                .AddQueryExecutor()
                .AddCommandHandlers(domainAssemblies)
                .AddQueryHandlers(domainAssemblies)
                .AddScoped<IPersonRepository, SqlServerPersonRepository>()
                .AddScoped<SampleDbContext>()
                .AddScoped(provider => dbConfig)
                .BuildServiceProvider();

            services.IsolateDomainServices(domainServices);

            //// alternative (single service provider):
            //services
            //    .AddCommandExecutor()
            //    .AddQueryExecutor()
            //    .AddCommandHandlers(domainAssemblies)
            //    .AddQueryHandlers(domainAssemblies)
            //    .AddScoped<IPersonRepository, SqlServerPersonRepository>()
            //    .AddScoped<SampleDbContext>()
            //    .AddScoped(provider => dbConfig);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
