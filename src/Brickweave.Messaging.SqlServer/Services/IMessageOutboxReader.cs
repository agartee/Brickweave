using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Brickweave.Messaging.Models;

namespace Brickweave.Messaging.SqlServer.Services
{
    public interface IMessageOutboxReader
    {
        Task<IEnumerable<DomainMessageInfo>> GetNextBatch(int batchSize, int retryAfterMins, int maxRetries);
        Task ReportFailure(DomainMessageInfo domainMessage);
        Task Delete(DomainMessageId domainMessageId);
    }
}
