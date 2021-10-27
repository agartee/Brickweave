using System;
using System.Threading;
using System.Threading.Tasks;
using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.Services
{
    public class CommandProcessor : ICommandProcessor
    {
        private readonly ICommandQueue _commandQueue;
        private readonly ICommandStatusRepository _commandStatusRepository;
        private readonly IDispatcher _dispatcher;

        private readonly int _pollingIntervalSeconds;

        public CommandProcessor(ICommandQueue commandQueue, ICommandStatusRepository commandStatusRepository,
            IDispatcher dispatcher, int pollingIntervalSeconds = 0)
        {
            _commandQueue = commandQueue;
            _commandStatusRepository = commandStatusRepository;
            _dispatcher = dispatcher;
            _pollingIntervalSeconds = pollingIntervalSeconds > 0 ? pollingIntervalSeconds : 15;
        }

        public async Task ProcessCommandsAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var nextCommand = await _commandQueue.GetNext();

                if (nextCommand != null)
                    await ProcessCommand(nextCommand);

                await Task.Delay(_pollingIntervalSeconds * 1000, stoppingToken);
            }
        }

        private async Task ProcessCommand(CommandInfo nextCommand)
        {
            try
            {
                await _commandStatusRepository.ReportStartedAsync(nextCommand.Id);

                if (nextCommand != null)
                {
                    var result = await _dispatcher.DispatchCommandAsync(nextCommand.Value, nextCommand.Principal);

                    await _commandStatusRepository.ReportCompletedAsync(nextCommand.Id, result);
                }
            }
            catch (Exception ex)
            {
                await _commandStatusRepository.ReportErrorAsync(nextCommand.Id, ex);
            }
        }
    }
}
