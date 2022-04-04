using System.Linq;
using Brickweave.Cqrs.AspNetCore.Formatters;
using Brickweave.Domain;
using Brickweave.Domain.AspNetCore.ModelBinders;
using Brickweave.Domain.Serialization;
using Brickweave.EventStore;
using Brickweave.Serialization.DependencyInjection;
using EventSourcing.Domain.Common.Serialization;
using EventSourcing.Domain.Ideas.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AdvancedCqrs.WebApp
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
            var domainAssembly = typeof(Idea).Assembly;

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
                // EventSourcing.Domain.Common.Serialization) so that the
                // "Name" value object can be flattened when serialized and 
                // deserialized just like Brickweave.Domain.Id value objects.
                .AddJsonConverter(new NameConverter());
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
