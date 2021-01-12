using System;
using System.Collections.Concurrent;
using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli
{
    public class LongRunningExecutionStatusCache
    {
        private readonly ConcurrentDictionary<Guid, LongRunningExecutionStatus> _items
            = new ConcurrentDictionary<Guid, LongRunningExecutionStatus>();

        public void ReportStarted(Guid executionId)
        {
            _items.TryAdd(executionId, new RunningStatus(executionId, DateTime.UtcNow));
        }

        public void ReportCompleted(Guid executionId, object result)
        {
            _items.TryRemove(executionId, out var currentStatus);
            
            if(currentStatus is RunningStatus runningStatus)
                _items.TryAdd(executionId, new CompletedStatus(executionId, runningStatus.Started, DateTime.UtcNow, result));
        }

        public void ReportError(Guid executionId, Exception exception)
        {
            _items.TryRemove(executionId, out var currentStatus);

            if (currentStatus is RunningStatus runningStatus)
                _items.TryAdd(executionId, new ErrorStatus(executionId, runningStatus.Started, DateTime.UtcNow, exception));
        }

        public LongRunningExecutionStatus ReadStatus(Guid executionId)
        {
            if (_items.ContainsKey(executionId))
            {
                if (_items[executionId] is CompletedStatus || _items[executionId] is ErrorStatus)
                {
                    _items.TryRemove(executionId, out var result);
                    return result;
                }

                return _items[executionId];
            }

            return new NotFoundStatus(executionId);
        }
    }
}
