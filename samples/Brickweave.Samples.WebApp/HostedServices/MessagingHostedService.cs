using System;
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
        private const int REPEAT_AFTER_SECONDS = 5;

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<MessagingHostedService> _logger;
        
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        
        private int executionCount = 0;
        private Task _executingTask;
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
                TimeSpan.FromSeconds(REPEAT_AFTER_SECONDS));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            executionCount++;

            _logger.LogInformation($"Timed Hosted Service is working. Count: {executionCount}");

            _timer?.Change(Timeout.Infinite, 0);
            _executingTask = DoWorkAsync(_cancellationTokenSource.Token);
        }

        private async Task DoWorkAsync(object state)
        {
            using (var serviceScope = _scopeFactory.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<SamplesDbContext>();
                var messageOutboxReader = serviceScope.ServiceProvider.GetRequiredService<IMessageOutboxReader>();
                var domainMessenger = serviceScope.ServiceProvider.GetRequiredService<IDomainMessenger>();

                try
                {
                    var messages = await messageOutboxReader.GetNextBatch(10);

                    foreach (var message in messages)
                    {
                        try
                        {
                            // note: transaction scope does not work in this scenario.
                            // "Local transactions are not supported with other resource managers/DTC." (ServiceBus exception)

                            await messageOutboxReader.Delete(message.Id);
                            _logger.LogInformation($"Deleted message with Id \"{message.Id}\"");

                            await domainMessenger.SendAsync(message.DomainEvent);
                            _logger.LogInformation($"Sent message with Id \"{message.Id}\"");
                        }
                        catch(Exception ex)
                        {
                            _logger.LogError(ex, $"An error occurred while processing message with Id \"{message.Id}\": {ex.Message}");
                        }
                    }
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
                finally
                {
                    _timer.Change(TimeSpan.FromSeconds(REPEAT_AFTER_SECONDS), TimeSpan.FromMilliseconds(-1));
                }
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
