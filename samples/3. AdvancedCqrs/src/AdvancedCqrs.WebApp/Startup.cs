using System;
using System.Globalization;
using System.Linq;
using AdvancedCqrs.Domain.Things.Commands;
using AdvancedCqrs.Domain.Things.Models;
using AdvancedCqrs.Domain.Things.Services;
using AdvancedCqrs.SqlServer;
using AdvancedCqrs.SqlServer.Repositories;
using AdvancedCqrs.WebApp.ModelBinders;
using Brickweave.Cqrs;
using Brickweave.Cqrs.AspNetCore.DependencyInjection;
using Brickweave.Cqrs.AspNetCore.Formatters;
using Brickweave.Cqrs.Cli.DependencyInjection;
using Brickweave.Cqrs.DependencyInjection;
using Brickweave.Cqrs.SqlServer.DependencyInjection;
using Brickweave.Domain.AspNetCore.ModelBinders;
using Brickweave.Domain.Serialization;
using Brickweave.Serialization.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                // Add the PlainTextInputFormatter to use with the CLI-enabled
                // endpoint, as the example PowerShell client
                // (/scripts/cli-client-nosecurity.ps1) sends web requests as
                // "text/plain".
                options.InputFormatters.Add(new PlainTextInputFormatter());
                
                // Add custom model binding so that command/query objects can
                // be deserialized by the framework for use directly in the
                // controllers.
                options.ModelBinderProviders.Insert(0, new ThingModelBinderProvider());
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

                // the following converters are used to flatten data in CLI output
                settings.Converters.Add(new IdConverter());
            });
        }

        private void ConfigureDomainServices(IServiceCollection services)
        {
            var domainAssembly = typeof(Thing).Assembly;

            services.AddBrickweaveCqrs(domainAssembly)
                // Allow longer-running commands to be enqueued and processed
                // by a background service.
                .EnableLongRunningCommands<AdvancedCqrsDbContext>(
                    dbContext => dbContext.CommandQueue,
                    dbContext => dbContext.CommandStatus)
                // Commands in the queue that are completed or exited with an
                // error can be automatically cleaned up after the specified 
                // timespan. This configuration registers the service(s)
                // required to enable that process, but does not setup the
                // ASP.NET background service, as Brickweave can be used for
                // non-web projects as well.
                .EnableCommandCleanup(deleteCommandsAfter: TimeSpan.FromMinutes(2))
                // Add the ASP.NET background services for processing
                // long-running commands and command-queue cleanup.
                .AddLongRunningCommandBackgroundService(pollingInterval: TimeSpan.FromSeconds(15))
                .AddLongRunningCommandCustodianBackgroundService(pollingInterval: TimeSpan.FromSeconds(15));

            // Registers CLI services that are used to translate command text
            // (with args) as well as providing a hook for additional
            // application customizations (e.g. date parsing). Here you may
            // also override CLI command text for specific commands and/or
            // queries.
            services.AddBrickweaveCli(domainAssembly)
                // Add the file containing help documentation for domain model
                // "categories" (domain model type, not a specific action)
                .AddCategoryHelpFile("cli-categories.json")
                .SetDateParsingCulture(new CultureInfo("en-US"));

            // Add serialization/deserialization services for the long-running
            // command-queue (as well as other services in future demos).
            services.AddBrickweaveSerialization()
                // This is optional but will effect the JSON documents written
                // to the command-queue in this demo (CommandJson and
                // ResultJson).
                .AddJsonConverter(new IdConverter());

            services.AddDbContextFactory<AdvancedCqrsDbContext>(options =>
            {
                options.UseSqlServer(
                    Configuration.GetConnectionString("demo"),
                    sql => sql.CommandTimeout(120));

                if (Convert.ToBoolean(Configuration["logging:enableSensitiveDataLogging"]))
                    options.EnableSensitiveDataLogging();
            });
            services.AddDbContext<AdvancedCqrsDbContext>();

            services.AddScoped<IThingRepository, SqlServerThingRepository>();
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
