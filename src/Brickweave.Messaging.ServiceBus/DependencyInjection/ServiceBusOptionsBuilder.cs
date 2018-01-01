using System;
using System.Collections.Generic;
using Brickweave.Messaging.Serialization;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Brickweave.Messaging.ServiceBus.DependencyInjection
{
    public class ServiceBusOptionsBuilder
    {
        private readonly IServiceCollection _services;

        public ServiceBusOptionsBuilder(IServiceCollection services)
        {
            services
                .AddScoped<IDomainMessenger, ServiceBusDomainMessenger>()
                .AddScoped<IMessageSerializer, JsonMessageSerializer>();

            _services = services;
        }

        public ServiceBusOptionsBuilder ConfigureMessageSender(
            string connectionString, string topicOrQueue, RetryPolicy retryPolicy = null)
        {
            _services.AddSingleton<IMessageSender>(s => new MessageSender(connectionString, topicOrQueue, retryPolicy ?? RetryPolicy.Default));
            return this;
        }

        public ServiceBusOptionsBuilder AddUserPropertyStrategy<T>(
            Func<T, Dictionary<string, object>> propertiesFunc) where T : IDomainEvent
        {
            _services.AddScoped<IUserPropertyStrategy>(services => new UserPropertyStrategy<T>(propertiesFunc));
            return this;
        }

        public ServiceBusOptionsBuilder AddGlobalUserPropertyStrategy(params string[] propertyNames)
        {
            _services.AddScoped<IUserPropertyStrategy>(services => new GlobalUserPropertyStrategy(propertyNames));
            return this;
        }

        public ServiceBusOptionsBuilder AddUtf8Encoding()
        {
            _services.AddScoped<IMessageEncoder, Utf8Encoder>();
            return this;
        }

        public IServiceCollection Services()
        {
            return _services;
        }
    }
}