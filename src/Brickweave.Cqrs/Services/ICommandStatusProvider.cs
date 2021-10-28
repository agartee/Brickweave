using System;
using System.Threading.Tasks;
using Brickweave.Cqrs.Models;

namespace Brickweave.Cqrs.Services
{
    public interface ICommandStatusProvider
    {
        Task<IExecutionStatus> GetStatusAsync(Guid commandId);
    }
}
