using System;
using System.Collections.Generic;
using System.Linq;
using Brickweave.Cqrs.Cli.DependencyInjection;
using Brickweave.Cqrs.DependencyInjection;
using Brickweave.Domain.Serialization;
using Brickweave.EventStore.SqlServer;
using Brickweave.EventStore.SqlServer.DependencyInjection;
using Brickweave.Messaging.ServiceBus.DependencyInjection;
using Brickweave.Samples.Domain.Persons.Events;
using Brickweave.Samples.Domain.Persons.Services;
using Brickweave.Samples.Persistence.SqlServer.Repositories;
using Brickweave.Samples.WebApp.Formatters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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

            if (env.IsDevelopment())
                builder.AddUserSecrets<Startup>();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => { options.InputFormatters.Add(new PlainTextInputFormatter()); })
                .AddJsonOptions(options =>
                    options.SerializerSettings.Converters.Add(new IdConverter()))
                .AddJsonOptions(options =>
                    options.SerializerSettings.Formatting = Formatting.Indented);

            var domainAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName.StartsWith("Brickweave"))
                .Where(a => a.FullName.Contains("Domain"))
                .ToArray();

            var domainServices = new ServiceCollection()
                .AddCqrs(domainAssemblies)
                .AddEventStore(Configuration.GetConnectionString("eventStore"), domainAssemblies)
                .AddMessageBus(Configuration.GetConnectionString("serviceBus"), Configuration["serviceBusTopic"])
                    .WithGlobalUserPropertyStrategy("Id")
                    .WithUserPropertyStrategy<PersonCreated>(@event => new Dictionary<string, object> { ["LastName"] = @event.LastName })
                    .WithUtf8Encoding()
                    .Services()
                .AddScoped<IPersonRepository, SqlServerPersonRepository>()
                .BuildServiceProvider();

            services
                .AddCqrsExecutors(domainServices)
                .AddCli(domainAssemblies);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            
            app.UseMvc();

            CreateDatabase();
        }

        private void CreateDatabase()
        {
            var eventStoreContext = new EventStoreContext(new DbContextOptionsBuilder().UseSqlServer(Configuration.GetConnectionString("eventStore")).Options);
            eventStoreContext.Database.EnsureCreated();
        }
    }
}
