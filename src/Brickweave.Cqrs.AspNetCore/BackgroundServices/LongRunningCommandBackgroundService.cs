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
        private readonly ILogger<LongRunningCommandBackgroundService> _logger;

        public LongRunningCommandBackgroundService(IServiceProvider services, ILogger<LongRunningCommandBackgroundService> logger)
        {
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(LongRunningCommandBackgroundService)} Service running.");

            using var scope = _services.CreateScope();
            
            var scopedProcessingService =
                scope.ServiceProvider.GetService<ILongRunningCommandProcessor>();

            if(scopedProcessingService != null)
                await scopedProcessingService.ProcessCommandsAsync(stoppingToken);
        }
    }
}
