using System;
using System.Threading;
using System.Threading.Tasks;
using Brickweave.Cqrs.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brickweave.Cqrs.AspNetCore.BackgroundServices
{
    public class LongRunningCommandBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly TimeSpan _pollingInterval;
        private readonly ILogger<LongRunningCommandBackgroundService> _logger;

        public LongRunningCommandBackgroundService(IServiceProvider services, LongRunningCommandBackgroundServiceConfig config,
            ILogger<LongRunningCommandBackgroundService> logger)
        {
            _services = services;
            _pollingInterval = config.PollingInterval;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(LongRunningCommandBackgroundService)} Service running.");

            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _services.CreateScope();

                var scopedProcessingService = scope.ServiceProvider.GetService<ILongRunningCommandProcessor>();

                if (scopedProcessingService != null)
                    await scopedProcessingService.TryProcessNextCommandAsync();

                _logger.LogInformation($"Waiting { _pollingInterval } before executing again.");
                await Task.Delay((int) _pollingInterval.TotalMilliseconds, stoppingToken);
            }
        }
    }

    public class LongRunningCommandBackgroundServiceConfig
    {
        public LongRunningCommandBackgroundServiceConfig(TimeSpan pollingInterval)
        {
            PollingInterval = pollingInterval;
        }

        public TimeSpan PollingInterval { get; }
    }
}
