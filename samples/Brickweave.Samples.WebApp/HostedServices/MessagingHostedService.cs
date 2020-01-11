using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Brickweave.Messaging;
using Brickweave.Messaging.SqlServer;
using Brickweave.Samples.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brickweave.Samples.WebApp.HostedServices
{
    public class MessagingHostedService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private Task _executingTask;

        private readonly ILogger<MessagingHostedService> _logger;
        private Timer _timer;

        public MessagingHostedService(IServiceScopeFactory scopeFactory, ILogger<MessagingHostedService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(
                DoWork, 
                null, 
                TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            executionCount++;

            _logger.LogInformation(
                "Timed Hosted Service is working. Count: {Count}", executionCount);

            _timer?.Change(Timeout.Infinite, 0);
            _executingTask = DoWorkAsync(_cancellationTokenSource.Token);
        }

        private async Task DoWorkAsync(object state)
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<SamplesDbContext>();
                    var messageOutboxReader = scope.ServiceProvider.GetRequiredService<IMessageOutboxReader>();
                    var domainMessenger = scope.ServiceProvider.GetRequiredService<IDomainMessenger>();

                    var events = await messageOutboxReader.GetNextBatch(dbContext.MessageOutbox, 10);
                    
                    if(events.Any())
                        await domainMessenger.SendAsync(events);
                }
            }
            finally
            {
                _timer.Change(TimeSpan.FromSeconds(5), TimeSpan.FromMilliseconds(-1));
            }   
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);

            if (_executingTask == null)
                return;

            try
            {
                _cancellationTokenSource.Cancel();
            }
            finally
            {
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
            }
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _timer?.Dispose();
        }
    }
}
