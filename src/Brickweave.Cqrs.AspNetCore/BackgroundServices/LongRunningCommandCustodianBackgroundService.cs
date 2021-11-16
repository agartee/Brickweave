using System;
using System.Threading;
using System.Threading.Tasks;
using Brickweave.Cqrs.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brickweave.Cqrs.AspNetCore.BackgroundServices
{
    public class LongRunningCommandCustodianBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly TimeSpan _pollingInterval;
        private readonly ILogger<LongRunningCommandCustodianBackgroundService> _logger;

        public LongRunningCommandCustodianBackgroundService(IServiceProvider services, LongRunningCommandCustodianBackgroundServiceConfig config, 
            ILogger<LongRunningCommandCustodianBackgroundService> logger)
        {
            _services = services;
            _pollingInterval = config.PollingInterval;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(LongRunningCommandCustodianBackgroundService)} Service running.");

            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _services.CreateScope();

                var scopedProcessingService =
                    scope.ServiceProvider.GetService<ILongRunningCommandCostodian>();

                if (scopedProcessingService != null)
                    await scopedProcessingService.AttendAsync();

                _logger.LogInformation($"Waiting { _pollingInterval } before executing again.");
                await Task.Delay((int) _pollingInterval.TotalMilliseconds, stoppingToken);
            }
        }
    }

    public class LongRunningCommandCustodianBackgroundServiceConfig
    {
        public LongRunningCommandCustodianBackgroundServiceConfig(TimeSpan pollingInterval)
        {
            PollingInterval = pollingInterval;
        }

        public TimeSpan PollingInterval { get; }
    }
}
