using System;
using System.Threading.Tasks;
using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.Services
{
    public interface ICommandStatusRepository
    {
        Task ReportStartedAsync(Guid executionId);
        Task ReportCompletedAsync(Guid executionId, object result);
        Task ReportErrorAsync(Guid executionId, Exception exception);
        Task<IExecutionStatus> ReadStatusAsync(Guid executionId);
    }
}
