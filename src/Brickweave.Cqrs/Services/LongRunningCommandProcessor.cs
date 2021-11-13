using System;
using System.Threading;
using System.Threading.Tasks;
using Brickweave.Cqrs.Models;
using Microsoft.Extensions.Logging;

namespace Brickweave.Cqrs.Services
{
    public class LongRunningCommandProcessor : ILongRunningCommandProcessor
    {
        private readonly ICommandQueue _commandQueue;
        private readonly IEnqueuedCommandDispatcher _dispatcher;
        private readonly TimeSpan _pollingInterval;
        private readonly ILogger<LongRunningCommandProcessor> _logger;

        public LongRunningCommandProcessor(ICommandQueue commandQueue, IEnqueuedCommandDispatcher dispatcher, TimeSpan pollingInterval, 
            ILogger<LongRunningCommandProcessor> logger)
        {
            _commandQueue = commandQueue;
            _dispatcher = dispatcher;
            _pollingInterval = pollingInterval;
            _logger = logger;
        }

        public async Task ProcessCommandsAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Checking for enqueued command...");
                var nextCommand = await _commandQueue.GetNextAsync();

                if (nextCommand != null)
                    await ProcessCommand(nextCommand);
                else
                    _logger.LogInformation($"No enqueued commands found.");

                _logger.LogInformation($"Waiting { _pollingInterval } before executing again.");
                await Task.Delay((int) _pollingInterval.TotalMilliseconds, stoppingToken);
            }
        }

        private async Task ProcessCommand(CommandInfo nextCommand)
        {
            _logger.LogInformation($"Processing { nextCommand.Value.GetType() } command...");

            try
            {
                if (nextCommand != null)
                {
                    var result = await _dispatcher.ExecuteAsync(nextCommand.Value, nextCommand.Principal);

                    _logger.LogInformation($"Reporting { nextCommand.Value.GetType() } command with ID { nextCommand.Id } completed.");
                    await _commandQueue.ReportCompletedAsync(nextCommand.Id, result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Reporting exception thrown by { nextCommand.Value.GetType() } command with ID { nextCommand.Id }.");
                await _commandQueue.ReportExceptionAsync(nextCommand.Id, ex);
            }
        }
    }
}
