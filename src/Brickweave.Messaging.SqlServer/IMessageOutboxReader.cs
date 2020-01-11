using System.Collections.Generic;
using System.Threading.Tasks;
using Brickweave.Domain;
using Brickweave.Messaging.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.Messaging.SqlServer
{
    public interface IMessageOutboxReader
    {
        Task<IEnumerable<IDomainEvent>> GetNextBatch<TMessageData>(DbSet<TMessageData> dbSet, int batchSize)
             where TMessageData : MessageData, new();
    }
}
