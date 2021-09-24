using System;
using System.Linq;
using System.Reflection;
using BasicMessaging.Domain.Places.Services;
using BasicMessaging.SqlServer;
using BasicMessaging.SqlServer.Repositories;
using BasicMessaging.WebApp.HostedServices;
using Brickweave.Cqrs.DependencyInjection;
using Brickweave.Domain;
using Brickweave.Domain.Serialization;
using Brickweave.Messaging.ServiceBus.DependencyInjection;
using Brickweave.Messaging.SqlServer;
using Brickweave.Messaging.SqlServer.Entities;
using Brickweave.Serialization;
using Brickweave.Serialization.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BasicMessaging.WebApp
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
            services.AddControllersWithViews();

            var domainAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName.Contains("BasicMessaging.Domain"))
                .ToArray();

            services.AddCqrs(domainAssemblies);

            var shortHandTypes = domainAssemblies.SelectMany(a => a.ExportedTypes)
                .Where(t => typeof(IDomainEvent).IsAssignableFrom(t))
                .ToArray();

            services.AddBrickweaveSerialization(shortHandTypes)
                .AddJsonConverter(new IdConverter());

            services.AddMessageBus()
                .AddMessageSenderRegistration(
                    Configuration.GetConnectionString("serviceBus"),
                    Configuration["messaging:queue"], isDefault: true)
                .AddUtf8Encoding();

            services.AddDbContext<BasicMessagingDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("demo"),
                    sql => sql.MigrationsAssembly(GetMigrationAssemblyName()).CommandTimeout(120));

                if (Convert.ToBoolean(Configuration["logging:enableSensitiveDataLogging"]))
                    options.EnableSensitiveDataLogging();
            });

            services.AddTransient<IPlaceRepository, SqlServerPlaceRepository>();
            services.AddScoped<IMessageOutboxReader>(s =>
                new SqlServerMessageOutboxReader<BasicMessagingDbContext, MessageData>(
                    s.GetService<BasicMessagingDbContext>(),
                    dbContext => dbContext.MessageOutbox,
                    s.GetService<IDocumentSerializer>()));

            services.AddHostedService<MessagingHostedService>();
            services.AddScoped<IMessagingAdapter, ServiceBusMessagingAdapter>();
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static string GetMigrationAssemblyName()
        {
            return typeof(BasicMessagingDbContext).GetTypeInfo().Assembly.GetName().Name;
        }
    }
}
