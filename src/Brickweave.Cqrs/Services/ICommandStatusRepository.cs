using System;
using System.Threading.Tasks;
using Brickweave.Cqrs.Models;

namespace Brickweave.Cqrs.Services
{
    public interface ICommandStatusRepository
    {
        Task ReportStartedAsync(Guid commandId);
        Task ReportCompletedAsync(Guid commandId, object result);
        Task ReportErrorAsync(Guid commandId, Exception exception);
        Task<IExecutionStatus> ReadStatusAsync(Guid commandId);
    }
}
