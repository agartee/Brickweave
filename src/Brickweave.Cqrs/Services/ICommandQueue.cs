using System;
using System.Threading.Tasks;
using Brickweave.Cqrs.Models;

namespace Brickweave.Cqrs.Services
{
    public interface ICommandQueue
    {
        Task EnqueueCommandAsync(Guid commandId, ICommand executable, ClaimsPrincipalInfo user = null);
        Task<CommandInfo> GetNextAsync();
        Task ReportCompletedAsync(Guid commandId, object result = null);
        Task ReportExceptionAsync(Guid commandId, Exception exception);
        Task DeleteAsync(Guid commandId);
        Task DeleteOlderThanAsync(TimeSpan startedCommandAge);
    }
}
