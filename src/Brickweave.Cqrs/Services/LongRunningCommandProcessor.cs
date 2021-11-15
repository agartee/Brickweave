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
                var result = await _dispatcher.ExecuteAsync(nextCommand.Value, nextCommand.Principal);

                await _commandQueue.ReportCompletedAsync(nextCommand.Id, result);

                _logger.LogInformation($"Command with ID { nextCommand.Id } reported completed.");
            }
            catch (Exception ex)
            {
                await _commandQueue.ReportExceptionAsync(nextCommand.Id, ex);

                _logger.LogInformation($"Command with ID { nextCommand.Id } reported exception thrown.");
            }
        }
    }
}
