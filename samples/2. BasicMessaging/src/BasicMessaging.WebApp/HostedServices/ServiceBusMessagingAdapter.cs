using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Brickweave.Messaging;
using Brickweave.Messaging.SqlServer;
using Brickweave.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BasicMessaging.WebApp.HostedServices
{
    internal interface IMessagingAdapter
    {
        Task DoWork(CancellationToken stoppingToken);
    }

    internal class ServiceBusMessagingAdapter : IMessagingAdapter
    {
        private const int REPEAT_AFTER_SECONDS = 5;

        private int executionCount = 0;
        private readonly IMessageOutboxReader _messageOutboxReader;
        private readonly IDomainMessenger _domainMessenger;
        private readonly ILogger _logger;
        private readonly IDocumentSerializer _documentSerializer; // used for demo logging

        public ServiceBusMessagingAdapter(IMessageOutboxReader messageOutboxReader,
            IDomainMessenger domainMessenger, ILogger<ServiceBusMessagingAdapter> logger, 
            IDocumentSerializer documentSerializer)
        {
            _messageOutboxReader = messageOutboxReader;
            _domainMessenger = domainMessenger;
            _logger = logger;
            _documentSerializer = documentSerializer;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"ServiceBus Messaging Adapter is working. Count: {executionCount}");

                executionCount++;

                try
                {
                    var messages = await _messageOutboxReader.GetNextBatch(10);

                    if (messages.Count() == 0)
                        _logger.LogInformation($"No messages found.");

                    foreach (var message in messages)
                    {
                        try
                        {
                            await _domainMessenger.SendAsync(message.DomainEvent);

                            var json = _documentSerializer.SerializeObject(message.DomainEvent);
                            _logger.LogInformation($"Sent message with Id \"{message.Id}\":\r\n{json}");

                            await _messageOutboxReader.Delete(message.Id);
                            _logger.LogInformation($"Deleted message with Id \"{message.Id}\"");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"An error occurred while processing message with Id \"{message.Id}\": {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }

                await Task.Delay(REPEAT_AFTER_SECONDS * 1000, stoppingToken);
            }
        }
    }

    public class MessagingHostedService : BackgroundService
    {
        private readonly ILogger<MessagingHostedService> _logger;

        public MessagingHostedService(IServiceProvider services,
            ILogger<MessagingHostedService> logger)
        {
            Services = services;
            _logger = logger;
        }

        public IServiceProvider Services { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Messaging Hosted Service running.");

            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Messaging Hosted Service is working.");

            using (var scope = Services.CreateScope())
            {
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<IMessagingAdapter>();

                await scopedProcessingService.DoWork(stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Messaging Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}
