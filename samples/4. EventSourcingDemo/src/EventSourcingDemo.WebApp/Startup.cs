using Brickweave.Cqrs.AspNetCore.Formatters;
using Brickweave.Cqrs.DependencyInjection;
using Brickweave.Domain;
using Brickweave.Domain.AspNetCore.ModelBinders;
using Brickweave.Domain.Serialization;
using Brickweave.EventStore;
using Brickweave.EventStore.SqlServer.DependencyInjection;
using Brickweave.Messaging.ServiceBus.DependencyInjection;
using Brickweave.Serialization.DependencyInjection;
using EventSourcingDemo.Domain.Accounts.Models;
using EventSourcingDemo.Domain.Accounts.Services;
using EventSourcingDemo.Domain.Common.Serialization;
using EventSourcingDemo.Domain.Companies.Services;
using EventSourcingDemo.Domain.People.Services;
using EventSourcingDemo.SqlServer;
using EventSourcingDemo.SqlServer.Repositories;
using EventSourcingDemo.WebApp.ModelBinders;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace EventSourcingDemo.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureControllers(services);
            ConfigureDomainServices(services);
        }

        private void ConfigureControllers(IServiceCollection services)
        {
            services.AddControllersWithViews(options =>
            {
                options.InputFormatters.Add(new PlainTextInputFormatter());
                options.ModelBinderProviders.Insert(0, new PersonModelBinderProvider());
                options.ModelBinderProviders.Insert(0, new CompanyModelBinderProvider());
                options.ModelBinderProviders.Insert(0, new IdModelBinderProvider());
            })
            .AddNewtonsoftJson(options =>
            {
                var settings = options.SerializerSettings;

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
            });
        }

        private void ConfigureDomainServices(IServiceCollection services)
        {
            var domainAssembly = typeof(Account).Assembly;
            
            // Get a list of types from our domain assembly (or assemblies)
            // that will be serialized and deserialized using only the class
            // name instead of their fully qualified class names, thus reducing
            // their serialized event size in the database.
            var shortHandTypes = domainAssembly.ExportedTypes
                .Where(t => typeof(IEvent).IsAssignableFrom(t) || typeof(IDomainEvent).IsAssignableFrom(t))
                .ToArray();

            services.AddBrickweaveSerialization(shortHandTypes)
                .AddJsonConverter(new IdConverter())
                // Adding a project-specific JSON converter (from
                // EventSourcingDemo.Domain.Common.Serialization) so that the
                // "Name" value object can be flattened when serialized and 
                // deserialized just like Brickweave.Domain.Id value objects.
                .AddJsonConverter(new NameConverter());

            services.AddBrickweaveMessaging()
                .AddMessageSenderRegistration(
                    "default",
                    connectionString: Configuration.GetConnectionString("serviceBus"),
                    topicOrQueue: Configuration["messaging:queue"],
                    isDefault: true)
                .AddUtf8Encoding();

            services.AddBrickweaveCqrs(domainAssembly);
            services.AddBrickweaveEventStore();

            services.AddDbContextFactory<EventSourcingDemoDbContext>(options =>
            {
                options.UseSqlServer(
                    Configuration.GetConnectionString("demo"),
                    sql => sql.CommandTimeout(120));

                if (Convert.ToBoolean(Configuration["logging:enableSensitiveDataLogging"]))
                    options.EnableSensitiveDataLogging();
            });
            services.AddDbContext<EventSourcingDemoDbContext>();

            services.AddScoped<IAccountRepository, SqlServerAccountRepository>();
            services.AddScoped<IPersonRepository, SqlServerPersonRepository>();
            services.AddScoped<ICompanyRepository, SqlServerCompanyRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
