using System;
using System.Threading.Tasks;
using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.Repositories
{
    public interface IExecutionStatusRepository
    {
        Task ReportStartedAsync(Guid executionId);
        Task ReportCompletedAsync(Guid executionId, object result);
        Task ReportErrorAsync(Guid executionId, Exception exception);
        Task<IExecutionStatus> ReadStatusAsync(Guid executionId);
    }
}