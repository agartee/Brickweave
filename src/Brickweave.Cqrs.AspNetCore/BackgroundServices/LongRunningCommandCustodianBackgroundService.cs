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
        private readonly ILogger<LongRunningCommandCustodianBackgroundService> _logger;

        public LongRunningCommandCustodianBackgroundService(IServiceProvider services, ILogger<LongRunningCommandCustodianBackgroundService> logger)
        {
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(LongRunningCommandCustodianBackgroundService)} Service running.");

            using var scope = _services.CreateScope();

            var scopedProcessingService =
                scope.ServiceProvider.GetService<ILongRunningCommandCostodian>();

            if (scopedProcessingService != null)
                await scopedProcessingService.AttendAsync(stoppingToken);
        }
    }
}
