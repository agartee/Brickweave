using System;
using System.Globalization;
using System.Linq;
using BasicCqrs.Domain.People.Queries;
using BasicCqrs.Domain.People.Services;
using Brickweave.Cqrs.AspNetCore.Formatters;
using Brickweave.Cqrs.Cli.DependencyInjection;
using Brickweave.Cqrs.DependencyInjection;
using Brickweave.Domain.AspNetCore.ModelBinders;
using Brickweave.Domain.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BasicCqrs.WebApp
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
            services.AddControllersWithViews(config =>
            {
                // Add the PlainTextInputFormatter to use with the CLI-enabled
                // endpoint, as the example PowerShell client
                // (/scripts/cli-client-nosecurity.ps1) sends web requests as
                // "text/plain".
                config.InputFormatters.Add(new PlainTextInputFormatter());

                // Add custom model binding so that ID value objects can be
                // deserialized by the framework for use directly in the
                // controllers.
                config.ModelBinderProviders.Insert(0, new IdModelBinderProvider());
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

                // The following converters are used to flatten data in CLI
                // output
                settings.Converters.Add(new IdConverter());
            }); ;

            // Brickweave supports wiring up multiple domain libraries. Simply
            // assemble all of the libraries you want to include into an array
            // and pass that array to the IServiceCollection extensions methods
            // below.
            var domainAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName.Contains("BasicCqrs.Domain"))
                .ToArray();

            // Registers common CQRS services (e.g. IDispatcher) as well as the
            // command handlers found in the provided domain assemblies
            // collection.
            services.AddBrickweaveCqrs(domainAssemblies);

            // Registers CLI services that are used to translate command text
            // (with args) as well as providing a hook for additional
            // application customizations (e.g. date parsing). Here you may
            // also override CLI command text for specific commands and/or
            // queries.
            services.AddBrickweaveCli(domainAssemblies)
                // the file containing help documentation for domain model
                // "categories" (domain model type, not a specific action)
                .AddCategoryHelpFile("cli-categories.json")
                .SetDateParsingCulture(new CultureInfo("en-US"))
                // Override the auto-discovered CLI command, changing the CLI
                // command from "people list" to "person list"
                .OverrideQueryName<ListPeople>("list", "person");

            // Register your application's domain services as per usual
            services.AddSingleton<IPersonRepository, DummyPersonRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
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
