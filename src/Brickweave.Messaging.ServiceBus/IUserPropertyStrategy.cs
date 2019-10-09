using System.Collections.Generic;
using Brickweave.Domain;

namespace Brickweave.Messaging.ServiceBus
{
    public interface IUserPropertyStrategy
    {
        Dictionary<string, object> GetUserProperties(IDomainEvent domainEvent);
    }
}
