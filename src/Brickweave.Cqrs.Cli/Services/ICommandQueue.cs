using System;
using System.Threading.Tasks;
using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.Services
{
    public interface ICommandQueue
    {
        Task EnqueueCommandAsync(Guid commandId, ICommand executable, ClaimsPrincipalInfo user = null);
        Task<CommandInfo> GetNext();
        Task Delete(Guid commandId);
    }
}
