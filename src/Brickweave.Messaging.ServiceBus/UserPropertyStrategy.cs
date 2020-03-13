using System;
using System.Collections.Generic;
using Brickweave.Domain;

namespace Brickweave.Messaging.ServiceBus
{
    public class UserPropertyStrategy<T> : IUserPropertyStrategy where T : IDomainEvent
    {
        private readonly Func<T, Dictionary<string, object>> _propertiesFunc;

        public UserPropertyStrategy(Func<T, Dictionary<string, object>> propertiesFunc)
        {
            _propertiesFunc = propertiesFunc;
        }

        public Dictionary<string, object> GetUserProperties(IDomainEvent domainEvent)
        {
            return domainEvent.GetType() == typeof(T) 
                ? _propertiesFunc.Invoke((T) domainEvent) 
                : new Dictionary<string, object>();
        }
    }
}