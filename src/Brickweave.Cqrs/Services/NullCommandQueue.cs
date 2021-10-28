using System;
using System.Threading.Tasks;
using Brickweave.Cqrs.Models;

namespace Brickweave.Cqrs.Services
{
    public class NullCommandQueue : ICommandQueue
    {
        public Task Delete(Guid commandId)
        {
            return Task.CompletedTask;
        }

        public Task EnqueueCommandAsync(Guid commandId, ICommand executable, ClaimsPrincipalInfo user = null)
        {
            return Task.CompletedTask;
        }

        public Task<CommandInfo> GetNext()
        {
            return null;
        }
    }
}
