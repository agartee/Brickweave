using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Brickweave.Cqrs.Services
{
    public class LongRunningCommandCostodian : ILongRunningCommandCostodian
    {
        private readonly ICommandQueue _commandQueue;
        private readonly TimeSpan _pollingInterval;
        private readonly TimeSpan _deleteCommandsAfter;
        private readonly ILogger<LongRunningCommandCostodian> _logger;

        public LongRunningCommandCostodian(ICommandQueue commandQueue, TimeSpan pollingInterval, TimeSpan deleteCommandsAfter, 
            ILogger<LongRunningCommandCostodian> logger)
        {
            _commandQueue = commandQueue;
            _pollingInterval = pollingInterval;
            _deleteCommandsAfter = deleteCommandsAfter;
            _logger = logger;
        }

        public async Task AttendAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Deleting abandoned commands/status from the queue.");
                await _commandQueue.DeleteOlderThanAsync(_deleteCommandsAfter);

                _logger.LogInformation($"Waiting { _pollingInterval } before executing again.");
                await Task.Delay((int)_pollingInterval.TotalMilliseconds, stoppingToken);
            }
        }
    }
}
