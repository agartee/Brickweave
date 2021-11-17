using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Brickweave.Cqrs.Services
{
    public class LongRunningCommandCostodian : ILongRunningCommandCostodian
    {
        private readonly ICommandQueue _commandQueue;
        private readonly TimeSpan _deleteCommandsAfter;
        private readonly ILogger<LongRunningCommandCostodian> _logger;

        public LongRunningCommandCostodian(ICommandQueue commandQueue, TimeSpan deleteCommandsAfter, 
            ILogger<LongRunningCommandCostodian> logger)
        {
            _commandQueue = commandQueue;
            _deleteCommandsAfter = deleteCommandsAfter;
            _logger = logger;
        }

        public async Task AttendAsync()
        {
            _logger.LogInformation($"Deleting abandoned commands/status from the queue.");
            await _commandQueue.DeleteOlderThanAsync(_deleteCommandsAfter);
        }
    }
}
