using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Brickweave.Domain;
using Brickweave.Messaging.ServiceBus.Extensions;

namespace Brickweave.Messaging.ServiceBus
{
    public class GlobalUserPropertyStrategy : IUserPropertyStrategy
    {
        private readonly string[] _userPropertyNames;

        public GlobalUserPropertyStrategy(params string[] userPropertyNames)
        {
            _userPropertyNames = userPropertyNames;
        }

        public Dictionary<string, object> GetUserProperties(IDomainEvent domainEvent)
        {
            var properties = new Dictionary<string, object>();

            var results = domainEvent.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => _userPropertyNames.Contains(p.Name, StringComparer.InvariantCultureIgnoreCase))
                .ToList();

            foreach(var p in results)
            {
                properties.Add(p.Name, p.PropertyType.IsBasicType()
                    ? p.GetValue(domainEvent) 
                    : p.GetValue(domainEvent).ToString());
            }

            return properties;
        }
    }
}
