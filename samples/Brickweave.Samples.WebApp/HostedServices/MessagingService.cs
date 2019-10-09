using System;
using System.Threading;
using System.Threading.Tasks;
using Brickweave.Messaging.SqlServer;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Brickweave.Samples.WebApp.HostedServices
{
    public class MessagingService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<MessagingService> _logger;
        private readonly IMessageOutboxReader _messageOutboxReader;
        private Timer _timer;

        public MessagingService(ILogger<MessagingService> logger, IMessageOutboxReader messageOutboxReader)
        {
            _logger = logger;
            _messageOutboxReader = messageOutboxReader;
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
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
