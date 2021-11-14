using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Brickweave.Messaging;
using Brickweave.Messaging.SqlServer;
using Microsoft.Extensions.Logging;

namespace BasicMessaging.WebApp.HostedServices
{

    internal class ServiceBusMessagingAdapter : IMessagingAdapter
    {
        private const int BATCH_SIZE = 5;
        private const int REPEAT_AFTER_SECONDS = 5;
        private const int MAX_RETRIES = 3;
        private const int RETRY_AFTER_SECONDS = 15;

        private readonly IMessageOutboxReader _messageOutboxReader;
        private readonly IDomainMessenger _domainMessenger;
        private readonly ILogger _logger;
        
        private int executionCount = 0;

        public ServiceBusMessagingAdapter(IMessageOutboxReader messageOutboxReader,
            IDomainMessenger domainMessenger, ILogger<ServiceBusMessagingAdapter> logger)
        {
            _messageOutboxReader = messageOutboxReader;
            _domainMessenger = domainMessenger;
            _logger = logger;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"ServiceBus Messaging Adapter is working. Count: {executionCount}");

                executionCount++;

                try
                {
                    var messages = await _messageOutboxReader.GetNextBatch(
                        BATCH_SIZE,
                        RETRY_AFTER_SECONDS, 
                        MAX_RETRIES);

                    if (messages.Count() == 0)
                        _logger.LogInformation($"No messages found.");

                    foreach (var message in messages)
                    {
                        try
                        {
                            await _domainMessenger.SendAsync(message.DomainEvent);
                            await _messageOutboxReader.Delete(message.Id);

                            _logger.LogInformation($"Sent message with Id \"{message.Id}\"");
                        }
                        catch (Exception ex)
                        {
                            await _messageOutboxReader.ReportFailure(message);

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
}
