using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brickweave.Messaging.Serialization;
using Brickweave.Messaging.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.Messaging.SqlServer.Extensions
{
    public class SqlServerDomainMessengerOutbox<TDbContext, TMessageData> : IDomainMessengerOutbox 
        where TDbContext : DbContext 
        where TMessageData : MessageData, new()
    {
        private readonly IMessageSerializer _serializer;
        private readonly TDbContext _dbContext;
        private readonly DbSet<TMessageData> _messagesDbSet;

        public SqlServerDomainMessengerOutbox(IMessageSerializer serializer, TDbContext dbContext, 
            Func<TDbContext, DbSet<TMessageData>> getMessagesDbSet)
        {
            _serializer = serializer;
            _dbContext = dbContext;
            _messagesDbSet = getMessagesDbSet.Invoke(_dbContext);
        }

        public async Task EnqueueAsync(IDomainEvent @event)
        {
            await EnqueueAsync(new List<IDomainEvent> { @event });
        }

        public async Task EnqueueAsync(params IDomainEvent[] events)
        {
            await EnqueueAsync(events.ToList());
        }

        public async Task EnqueueAsync(IEnumerable<IDomainEvent> events)
        {
            var data = events.Select((e, i) => new TMessageData
            {
                Id = Guid.NewGuid(),
                Message = _serializer.SerializeObject(e),
                Created = DateTime.Now,
                CommitSequence = i
            });

            _messagesDbSet.AddRange(data);

            await _dbContext.SaveChangesAsync();
        }
    }
}
