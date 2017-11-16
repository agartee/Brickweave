using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Brickweave.Cqrs.Cli.DependencyInjection;
using Brickweave.Cqrs.DependencyInjection;
using Brickweave.Domain.Serialization;
using Brickweave.EventStore.SqlServer;
using Brickweave.EventStore.SqlServer.DependencyInjection;
using Brickweave.Messaging.ServiceBus.DependencyInjection;
using Brickweave.Samples.Domain.Persons.Events;
using Brickweave.Samples.Domain.Persons.Services;
using Brickweave.Samples.Persistence.SqlServer;
using Brickweave.Samples.Persistence.SqlServer.Repositories;
using Brickweave.Samples.WebApp.Formatters;
using Brickweave.Samples.WebApp.Logging;
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
            ContentRootPath = env.ContentRootPath;
        }

        public IConfigurationRoot Configuration { get; }
        public string ContentRootPath { get; }

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

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services
                .AddCqrs(domainAssemblies)
                .AddEventStore(domainAssemblies)
                    .AddDbContext(options => options.UseSqlServer(Configuration.GetConnectionString("brickweave_samples"),
                        sql => sql.MigrationsAssembly(migrationsAssembly)))
                    .Services()
                .AddMessageBus()
                    .ConfigureMessageSender(Configuration.GetConnectionString("serviceBus"), Configuration["serviceBusTopic"])
                    .AddGlobalUserPropertyStrategy("Id")
                    .AddUserPropertyStrategy<PersonCreated>(@event => new Dictionary<string, object> { ["LastName"] = @event.LastName })
                    .AddUtf8Encoding()
                    .Services()
                .AddCli(domainAssemblies)
                    .AddCategoryHelpFile("cli-categories.json");

            services.AddDbContext<SamplesContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("brickweave_samples"),
                    sql => sql.MigrationsAssembly(migrationsAssembly)));

            services
                .AddScoped<IPersonRepository, SqlServerPersonRepository>()
                .AddScoped<IPersonInfoRepository, SqlServerPersonRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            if (env.IsDevelopment())
            {
                loggerFactory.AddDebug();
                loggerFactory.AddProvider(new MyLoggerProvider());
            }
            
            app.UseMvc();

            app.ApplicationServices.GetService<EventStoreContext>().Database.Migrate();
            app.ApplicationServices.GetService<SamplesContext>().Database.Migrate();
        }
    }
}
