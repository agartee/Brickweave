using System.Collections.Generic;

namespace Brickweave.Messaging.ServiceBus
{
    public interface IUserPropertyStrategy
    {
        Dictionary<string, object> GetUserProperties(IDomainEvent domainEvent);
    }
}
