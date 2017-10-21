using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Brickweave.Messaging.ServiceBus.DependencyInjection
{
    public class ServiceBusDependencyBuilder
    {
        private readonly IServiceCollection _services;

        public ServiceBusDependencyBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public ServiceBusDependencyBuilder WithUserPropertyStrategy<T>(
            Func<T, Dictionary<string, object>> propertiesFunc) where T : IDomainEvent
        {
            _services.AddScoped<IUserPropertyStrategy>(services => new UserPropertyStrategy<T>(propertiesFunc));
            return this;
        }

        public ServiceBusDependencyBuilder WithGlobalUserPropertyStrategy(params string[] propertyNames)
        {
            _services.AddScoped<IUserPropertyStrategy>(services => new GlobalUserPropertyStrategy(propertyNames));
            return this;
        }

        public ServiceBusDependencyBuilder WithUtf8Encoding()
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