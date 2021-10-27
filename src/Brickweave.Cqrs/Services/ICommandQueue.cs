using System;
using System.Threading.Tasks;
using Brickweave.Cqrs.Models;

namespace Brickweave.Cqrs.Services
{
    public interface ICommandQueue
    {
        Task EnqueueCommandAsync(Guid commandId, ICommand executable, ClaimsPrincipalInfo user = null);
        Task<CommandInfo> GetNext();
        Task Delete(Guid commandId);
    }
}
