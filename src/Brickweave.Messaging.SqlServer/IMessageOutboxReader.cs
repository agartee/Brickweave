using System.Collections.Generic;
using System.Threading.Tasks;
using Brickweave.Messaging.Models;

namespace Brickweave.Messaging.SqlServer
{
    public interface IMessageOutboxReader
    {
        Task<IEnumerable<DomainMessageInfo>> GetNextBatch(int batchSize);
        Task Delete(DomainMessageId domainMessageId);
    }
}
