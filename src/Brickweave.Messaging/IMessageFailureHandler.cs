using System;
using System.Threading.Tasks;
using Brickweave.Domain;

namespace Brickweave.Messaging
{
    public interface IMessageFailureHandler
    {
        Task Handle(IDomainEvent @event, Exception ex);
    }
}
