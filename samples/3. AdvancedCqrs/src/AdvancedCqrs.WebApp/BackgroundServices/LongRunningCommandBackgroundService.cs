using System;
using System.Threading;
using System.Threading.Tasks;
using Brickweave.Cqrs.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AdvancedCqrs.WebApp.BackgroundServices
{
    public class LongRunningCommandBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;

        public LongRunningCommandBackgroundService(IServiceProvider services)
        {
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _services.CreateScope();
            
            var scopedProcessingService =
                scope.ServiceProvider.GetService<ICommandProcessor>();

            if(scopedProcessingService != null)
                await scopedProcessingService.ProcessCommandsAsync(stoppingToken);
        }
    }
}
