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

        public Task<CommandInfo> GetNextAsync()
        {
            return Task.FromResult((CommandInfo) null);
        }

        public Task ReportCompletedAsync(Guid commandId, object result = null)
        {
            return Task.CompletedTask;
        }

        public Task ReportExceptionAsync(Guid commandId, Exception exception)
        {
            return Task.CompletedTask;
        }

        public Task ReportStartedAsync(Guid commandId)
        {
            return Task.CompletedTask;
        }
    }
}
