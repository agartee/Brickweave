using System;
using System.Threading.Tasks;

namespace Brickweave.Messaging
{
    public interface IMessageFailureHandler
    {
        Task Handle(IDomainEvent @event, Exception ex);
    }
}
