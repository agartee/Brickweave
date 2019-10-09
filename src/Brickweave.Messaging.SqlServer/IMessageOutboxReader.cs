using System.Collections.Generic;
using System.Threading.Tasks;
using Brickweave.Domain;

namespace Brickweave.Messaging.SqlServer
{
    public interface IMessageOutboxReader
    {
        Task<IEnumerable<IDomainEvent>> GetNextBatch(int batchSize);
    }
}
