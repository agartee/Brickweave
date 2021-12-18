using System;
using System.Globalization;
using AdvancedCqrs.Domain.Things.Models;
using AdvancedCqrs.Domain.Things.Services;
using AdvancedCqrs.SqlServer;
using AdvancedCqrs.SqlServer.Repositories;
using AdvancedCqrs.WebApp.ModelBinders;
using Brickweave.Cqrs.AspNetCore.DependencyInjection;
using Brickweave.Cqrs.AspNetCore.Formatters;
using Brickweave.Cqrs.Cli.DependencyInjection;
using Brickweave.Cqrs.DependencyInjection;
using Brickweave.Cqrs.SqlServer.DependencyInjection;
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
                options.InputFormatters.Add(new PlainTextInputFormatter());
                options.ModelBinderProviders.Insert(0, new ThingCommandModelBinderProvider());
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
                .EnableLongRunningCommands<AdvancedCqrsDbContext>(
                    dbContext => dbContext.CommandQueue,
                    dbContext => dbContext.CommandStatus)
                .EnableCommandCleanup(deleteCommandsAfter: TimeSpan.FromMinutes(5))
                .AddLongRunningCommandBackgroundService(pollingInterval: TimeSpan.FromSeconds(15))
                .AddLongRunningCommandCustodianBackgroundService(pollingInterval: TimeSpan.FromSeconds(15));

            services.AddBrickweaveCli(domainAssembly)
                .SetDateParsingCulture(new CultureInfo("en-US"))
                .AddCategoryHelpFile("cli-categories.json");

            services.AddBrickweaveSerialization();

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
