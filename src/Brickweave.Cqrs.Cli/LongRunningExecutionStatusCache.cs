using System;
using System.Collections.Concurrent;
using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli
{
    public class LongRunningExecutionStatusCache
    {
        private readonly ConcurrentDictionary<Guid, LongRunningExecutionStatus> _items
            = new ConcurrentDictionary<Guid, LongRunningExecutionStatus>();

        public void ReportCompleted(Guid executionId, LongRunningExecutionStatus result)
        {
            _items.TryAdd(executionId, result);
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
