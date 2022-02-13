using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BasicMessaging.Domain.Places.Events;
using BasicMessaging.Domain.Places.Models;
using BasicMessaging.Domain.Places.Services;
using BasicMessaging.SqlServer;
using BasicMessaging.SqlServer.Repositories;
using BasicMessaging.WebApp.HostedServices;
using Brickweave.Cqrs.DependencyInjection;
using Brickweave.Domain;
using Brickweave.Domain.Serialization;
using Brickweave.Messaging.ServiceBus.DependencyInjection;
using Brickweave.Messaging.SqlServer.Entities;
using Brickweave.Messaging.SqlServer.Services;
using Brickweave.Serialization;
using Brickweave.Serialization.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            var domainAssembly = typeof(Place).Assembly;

            services.AddBrickweaveCqrs(domainAssembly);

            // Get the types that when serialized or deserialized can drop
            // their fully qualified namespace names when written to the
            // message-outbox table's TypeName column or message-bus's
            // Content-Type property.
            var shortHandTypes = domainAssembly.ExportedTypes
                .Where(t => typeof(IDomainEvent).IsAssignableFrom(t))
                .ToArray();

            services.AddBrickweaveSerialization(shortHandTypes)
                // Include the Brickweave converter that will flatten Id<T>
                // types to their Value property when serialized or
                // deserialized.
                .AddJsonConverter(new IdConverter());

            services.AddBrickweaveMessaging()
                // Multiple message senders can be configured. One should be
                // configured as the default unless ALL message types have
                // their own explicit registrations.
                .AddMessageSenderRegistration(
                    "default",
                    connectionString: Configuration.GetConnectionString("serviceBus"),
                    topicOrQueue: Configuration["messaging:queue"], 
                    isDefault: true)
                // Azure Message Bus requires encoding of some type.
                // Currently only a UTF-8 encode is available with this
                // Brickweave library.
                .AddUtf8Encoding()
                // Tell the Brickweave services that any message that contains
                // an Id property should also add that property/value to the 
                // user properties (custom properties) of the outbound message.
                .AddGlobalUserPropertyStrategy("Id")
                // Message-type to topic/queue mapping in the cases where
                // multiple topics/queues are utilized. This is not technically
                // required for this demo, since a default message sender has
                // been defined above, but is included for reference.
                .AddMessageTypeRegistration<PlaceCreated>(
                    messageSenderName: "default")
                // Provide additional user property (custom property) mappings
                // at a per-message-type basis.
                .AddUserPropertyStrategy<PlaceCreated>(e => 
                    new Dictionary<string, object>
                    {
                        ["Name"] = e.Name
                    });

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
            app.UseDeveloperExceptionPage();
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
