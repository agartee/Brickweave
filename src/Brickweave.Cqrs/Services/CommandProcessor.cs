using System;
using System.Threading;
using System.Threading.Tasks;
using Brickweave.Cqrs.Models;

namespace Brickweave.Cqrs.Services
{
    public class CommandProcessor : ICommandProcessor
    {
        private readonly ICommandQueue _commandQueue;
        private readonly IEnqueuedCommandDispatcher _dispatcher;

        private readonly int _pollingIntervalSeconds;

        public CommandProcessor(ICommandQueue commandQueue, IEnqueuedCommandDispatcher dispatcher, int pollingIntervalSeconds = 0)
        {
            _commandQueue = commandQueue;
            _dispatcher = dispatcher;
            _pollingIntervalSeconds = pollingIntervalSeconds > 0 ? pollingIntervalSeconds : 15;
        }

        public async Task ProcessCommandsAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var nextCommand = await _commandQueue.GetNextAsync();

                if (nextCommand != null)
                    await ProcessCommand(nextCommand);

                await Task.Delay(_pollingIntervalSeconds * 1000, stoppingToken);
            }
        }

        private async Task ProcessCommand(CommandInfo nextCommand)
        {
            try
            {
                if (nextCommand != null)
                {
                    var result = await _dispatcher.ExecuteAsync(nextCommand.Value, nextCommand.Principal);

                    await _commandQueue.ReportCompletedAsync(nextCommand.Id, result);
                }
            }
            catch (Exception ex)
            {
                await _commandQueue.ReportExceptionAsync(nextCommand.Id, ex);
            }
        }
    }
}
