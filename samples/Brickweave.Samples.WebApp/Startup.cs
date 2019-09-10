using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Brickweave.Cqrs.Cli.DependencyInjection;
using Brickweave.Cqrs.DependencyInjection;
using Brickweave.Domain.Serialization;
using Brickweave.EventStore.SqlServer.DependencyInjection;
using Brickweave.Samples.Domain.Persons.Commands;
using Brickweave.Samples.Domain.Persons.Queries;
using Brickweave.Samples.Domain.Persons.Services;
using Brickweave.Samples.SqlServer;
using Brickweave.Samples.SqlServer.Repositories;
using Brickweave.Samples.WebApp.Formatters;
using Brickweave.Samples.WebApp.Middleware;
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
            
            services.AddCqrs(domainAssemblies);

            services.AddEventStore(domainAssemblies)
                .AddJsonConverter(new IdConverter());

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

            services
                .AddScoped<IPersonRepository, SqlServerPersonRepository>()
                .AddScoped<IPersonInfoRepository, SqlServerPersonRepository>()
                .AddScoped<IPersonEventStreamRepository, SqlServerPersonRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAuthentication();
            app.UseMvc();
            app.UseExceptionHandlingMiddleware();

            app.ApplicationServices.GetService<SamplesDbContext>().Database.Migrate();
        }

        private static string GetMigrationAssemblyName()
        {
            return typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
        }
    }
}
