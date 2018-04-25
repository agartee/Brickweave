using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Brickweave.Cqrs.Cli.DependencyInjection;
using Brickweave.Cqrs.DependencyInjection;
using Brickweave.Domain.Serialization;
using Brickweave.EventStore.SqlServer.DependencyInjection;
using Brickweave.Messaging.ServiceBus.DependencyInjection;
using Brickweave.Samples.Domain.Persons.Events;
using Brickweave.Samples.Domain.Persons.Queries;
using Brickweave.Samples.Domain.Persons.Services;
using Brickweave.Samples.Persistence.SqlServer;
using Brickweave.Samples.Persistence.SqlServer.Repositories;
using Brickweave.Samples.Projection;
using Brickweave.Samples.WebApp.Formatters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
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
            ConfigureMvc(services);
            ConfigureSecurity(services);
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
                    settings.Formatting = Formatting.Indented;
                    settings.Converters.Add(new IdConverter());
                });
        }

        protected virtual void ConfigureSecurity(IServiceCollection services)
        {
            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = Configuration["security:authority"];
                    options.RequireHttpsMetadata = Convert.ToBoolean(Configuration["security:requireHttpsMetadata"]);
                    options.ApiName = Configuration["security:apiName"];
                });
        }

        private void ConfigureBrickweave(IServiceCollection services)
        {
            var domainAssemblies = GetDomainAssemblies();
            
            services.AddCqrs(domainAssemblies);
            services.AddEventStore(domainAssemblies);
            services.AddMessageBus()
                .ConfigureMessageSender(
                    Configuration.GetConnectionString("serviceBus"),
                    Configuration["serviceBusTopic"])
                .AddGlobalUserPropertyStrategy("Id")
                .AddUserPropertyStrategy<PersonCreated>(@event =>
                    new Dictionary<string, object> {["LastName"] = @event.LastName})
                .AddUtf8Encoding();
            services.AddCli(domainAssemblies)
                .AddDateParsingCulture(new CultureInfo("en-US"))
                .AddCategoryHelpFile("cli-categories.json")
                .OverrideQueryName<ListPersons>("list", "person");
        }

        private static Assembly[] GetDomainAssemblies()
        {
            return DependencyContext.Default.RuntimeLibraries
                .Where(x => x.Name.StartsWith("Brickweave")
                    && (x.Name.Contains("Domain") || x.Name.Contains("Projection")))
                .Select(x => Assembly.Load(new AssemblyName(x.Name)))
                .ToArray();
        }

        private void ConfigureCustomServices(IServiceCollection services)
        {
            services.AddDbContext<SamplesDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("brickweave_samples"),
                    sql => sql.MigrationsAssembly(GetMigrationAssemblyName())));

            services.AddDbContext<SamplesProjectionDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("brickweave_samples_projection"),
                    sql => sql.MigrationsAssembly(GetMigrationAssemblyName())));

            services
                .AddScoped<IPersonRepository, SqlServerPersonRepository>()
                .AddScoped<IPersonInfoRepository, SqlServerPersonRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddDebug();

            app.UseAuthentication();
            app.UseMvc();

            app.ApplicationServices.GetService<SamplesDbContext>().Database.Migrate();
            app.ApplicationServices.GetService<SamplesProjectionDbContext>().Database.EnsureCreated();
            app.ApplicationServices.GetService<SamplesProjectionDbContext>().Database.Migrate();
        }

        private static string GetMigrationAssemblyName()
        {
            return typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
        }
    }
}
