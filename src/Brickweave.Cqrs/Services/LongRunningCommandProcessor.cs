using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Brickweave.Cqrs.Services
{
    public class LongRunningCommandProcessor : ILongRunningCommandProcessor
    {
        private readonly ICommandQueue _commandQueue;
        private readonly IEnqueuedCommandDispatcher _dispatcher;
        private readonly ILogger<LongRunningCommandProcessor> _logger;

        public LongRunningCommandProcessor(ICommandQueue commandQueue, IEnqueuedCommandDispatcher dispatcher, 
            ILogger<LongRunningCommandProcessor> logger)
        {
            _commandQueue = commandQueue;
            _dispatcher = dispatcher;
            _logger = logger;
        }

        public async Task TryProcessNextCommandAsync()
        {
            _logger.LogInformation($"Checking for enqueued command...");
            var nextCommand = await _commandQueue.GetNextAsync();
            
            if (nextCommand == null)
                return;

            var commandType = nextCommand.Value.GetType();

            try
            {
                _logger.LogInformation($"Command with ID { nextCommand.Id } ({ commandType }) found. Processing command...");
                var result = await _dispatcher.ExecuteAsync(nextCommand.Value, nextCommand.Principal);

                await _commandQueue.ReportCompletedAsync(nextCommand.Id, result);
                _logger.LogInformation($"Command with ID { nextCommand.Id } ({ commandType }) reported completed.");
            }
            catch (Exception ex)
            {
                await _commandQueue.ReportExceptionAsync(nextCommand.Id, ex);
                _logger.LogInformation($"Command with ID { nextCommand.Id } ({ commandType }) reported exception thrown.");
            }
        }
    }
}
