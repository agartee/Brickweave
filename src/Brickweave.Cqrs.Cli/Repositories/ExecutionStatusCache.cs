using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.Repositories
{
    public class ExecutionStatusCache : IExecutionStatusRepository
    {
        private readonly ConcurrentDictionary<Guid, ExecutionStatus> _items
            = new ConcurrentDictionary<Guid, ExecutionStatus>();

        public Task ReportStartedAsync(Guid executionId)
        {
            _items.TryAdd(executionId, new RunningStatus(executionId, DateTime.UtcNow));

            return Task.CompletedTask;
        }

        public Task ReportCompletedAsync(Guid executionId, object result)
        {
            _items.TryRemove(executionId, out var currentStatus);

            if (currentStatus is RunningStatus runningStatus)
                _items.TryAdd(executionId, new CompletedStatus(executionId, runningStatus.Started, DateTime.UtcNow, result));

            return Task.CompletedTask;
        }

        public Task ReportErrorAsync(Guid executionId, Exception exception)
        {
            _items.TryRemove(executionId, out var currentStatus);

            if (currentStatus is RunningStatus runningStatus)
                _items.TryAdd(executionId, new ErrorStatus(executionId, runningStatus.Started, DateTime.UtcNow, exception.Message));

            return Task.CompletedTask;
        }

        public Task<IExecutionStatus> ReadStatusAsync(Guid executionId)
        {
            if (_items.ContainsKey(executionId))
            {
                if (_items[executionId] is CompletedStatus || _items[executionId] is ErrorStatus)
                {
                    _items.TryRemove(executionId, out var result);
                    return Task.FromResult((IExecutionStatus) result);
                }

                return Task.FromResult((IExecutionStatus) _items[executionId]);
            }

            return Task.FromResult((IExecutionStatus) new NotFoundStatus(executionId));
        }
    }
}
