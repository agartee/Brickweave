using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brickweave.Messaging
{
    public interface IDomainMessengerOutbox
    {
        Task EnqueueAsync(IDomainEvent @event);
        Task EnqueueAsync(params IDomainEvent[] events);
        Task EnqueueAsync(IEnumerable<IDomainEvent> events);
    }
}
