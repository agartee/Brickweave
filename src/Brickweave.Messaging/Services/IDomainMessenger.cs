using System.Collections.Generic;
using System.Threading.Tasks;
using Brickweave.Domain;

namespace Brickweave.Messaging.Services
{
    public interface IDomainMessenger
    {
        Task SendAsync(IDomainEvent @event);
        Task SendAsync(IEnumerable<IDomainEvent> events);
        Task SendAsync(params IDomainEvent[] events);
    }
}
