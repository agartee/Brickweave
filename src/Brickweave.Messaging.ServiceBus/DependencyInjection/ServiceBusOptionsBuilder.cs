using System;
using System.Collections.Generic;
using Brickweave.Domain;
using Brickweave.Messaging.ServiceBus.Models;
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
                .AddScoped<IDomainMessenger, ServiceBusDomainMessenger>();

            _services = services;
        }

        public ServiceBusOptionsBuilder AddMessageSenderRegistration(
            string connectionString, string topicOrQueue, RetryPolicy retryPolicy = null, bool isDefault = false)
        {
            _services.AddSingleton(s => new MessageSenderRegistration(topicOrQueue, 
                new MessageSender(connectionString, topicOrQueue, retryPolicy ?? RetryPolicy.Default)));

            if(isDefault)
                _services.AddSingleton(new DefaultTopicOrQueueRegistration(topicOrQueue));

            return this;
        }

        public ServiceBusOptionsBuilder AddMessageTypeRegistration<TType>(string topicOrQueue) where TType : IDomainEvent
        {
            _services.AddSingleton<IMessageTypeRegistration>(s => new MessageTypeRegistration<TType>(topicOrQueue));

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

        public ServiceBusOptionsBuilder AddMessageFailureHandler<T>() where T : class, IMessageFailureHandler
        {
            _services.AddScoped<IMessageFailureHandler, T>();
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