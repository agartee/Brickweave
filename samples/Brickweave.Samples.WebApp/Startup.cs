using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Brickweave.Cqrs.Cli.DependencyInjection;
using Brickweave.Cqrs.DependencyInjection;
using Brickweave.Domain;
using Brickweave.Domain.Serialization;
using Brickweave.EventStore;
using Brickweave.EventStore.SqlServer.DependencyInjection;
using Brickweave.Messaging;
using Brickweave.Messaging.ServiceBus;
using Brickweave.Messaging.ServiceBus.DependencyInjection;
using Brickweave.Messaging.SqlServer;
using Brickweave.Messaging.SqlServer.Entities;
using Brickweave.Samples.Domain.Persons.Commands;
using Brickweave.Samples.Domain.Persons.Events;
using Brickweave.Samples.Domain.Persons.Queries;
using Brickweave.Samples.Domain.Persons.Services;
using Brickweave.Samples.SqlServer;
using Brickweave.Samples.SqlServer.Repositories;
using Brickweave.Samples.WebApp.Formatters;
using Brickweave.Samples.WebApp.HostedServices;
using Brickweave.Samples.WebApp.Middleware;
using Brickweave.Serialization;
using Brickweave.Serialization.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Brickweave.Samples.WebApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureMvc(services);
            ConfigureAuthentication(services);
            ConfigureBrickweave(services);
            ConfigureCustomServices(services);
        }

        private void ConfigureMvc(IServiceCollection services)
        {
            services.AddMvcCore(options =>
                {
                  options.InputFormatters.Add(new PlainTextInputFormatter());  
                })
                .AddAuthorization()
                .AddJsonFormatters(settings =>
                {
                    settings.TypeNameHandling = TypeNameHandling.None;

                    settings.ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy
                        {
                            ProcessDictionaryKeys = true
                        }
                    };

                    settings.Formatting = Formatting.Indented;

                    settings.Converters.Add(new IdConverter());
                    settings.Converters.Add(new StringEnumConverter());
                });
        }

        protected virtual void ConfigureAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = Configuration["authentication:authority"];
                options.Audience = Configuration["authentication:audience"];
            });
        }

        private void ConfigureBrickweave(IServiceCollection services)
        {
            var domainAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName.Contains("Samples.Domain"))
                .ToArray();

            var shortHandTypes = domainAssemblies.SelectMany(a => a.ExportedTypes)
                .Where(t => typeof(IEvent).IsAssignableFrom(t) || typeof(IDomainEvent).IsAssignableFrom(t))
                .ToArray();

            services.AddCqrs(domainAssemblies);

            services.AddBrickweaveSerialization(shortHandTypes)
                .AddJsonConverter(new IdConverter());

            services.AddEventStore();

            services.AddCli(domainAssemblies)
                .AddDateParsingCulture(new CultureInfo("en-US"))
                .AddCategoryHelpFile("cli-categories.json")
                .OverrideCommandName<AddPersonPhone>("add", "person", "phones")
                .OverrideCommandName<RemovePersonPhone>("remove", "person", "phones")
                .OverrideCommandName<UpdatePersonPhone>("update", "person", "phones")
                .OverrideCommandName<AddSinglePersonAttribute>("add-single", "person", "attributes")
                .OverrideCommandName<AddMultiplePersonAttributes>("add-multiple", "person", "attributes")
                .OverrideCommandName<RemoveSinglePersonAttribute>("remove", "person", "attributes")
                .OverrideQueryName<ListPeople>("list", "person")
                .OverrideQueryName<ExportPeople>("export-all", "person");
        }

        private void ConfigureCustomServices(IServiceCollection services)
        {
            services.AddDbContext<SamplesDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("brickweave_samples"),
                    sql => sql.MigrationsAssembly(GetMigrationAssemblyName())));

            services.AddMessageBus()
                .AddMessageSenderRegistration(
                    Configuration.GetConnectionString("serviceBus"),
                    Configuration["messaging:queue_samples"], isDefault: true)
                .AddGlobalUserPropertyStrategy("Id")
                .AddUserPropertyStrategy<PersonCreated>(@event =>
                    new Dictionary<string, object> { ["LastName"] = @event.LastName })
                .AddUtf8Encoding();

            services
                .AddScoped<IPersonRepository, SqlServerPersonRepository>()
                .AddScoped<IPersonInfoRepository, SqlServerPersonRepository>()
                .AddScoped<IPersonEventStreamRepository, SqlServerPersonRepository>()
                // todo: cleanup or add config builder
                .AddScoped<IMessageOutboxReader>(s => 
                    new SqlServerMessageOutboxReader<SamplesDbContext, MessageData>(
                        s.GetService<SamplesDbContext>(), 
                        dbContext => dbContext.MessageOutbox,
                        s.GetService<IDocumentSerializer>()));

            services.AddHostedService<MessagingHostedService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseExceptionHandlingMiddleware();

            app.UseAuthentication();
            app.UseMvc();
            
            app.ApplicationServices.GetService<SamplesDbContext>().Database.Migrate();
        }

        private static string GetMigrationAssemblyName()
        {
            return typeof(SamplesDbContext).GetTypeInfo().Assembly.GetName().Name;
        }
    }
}
