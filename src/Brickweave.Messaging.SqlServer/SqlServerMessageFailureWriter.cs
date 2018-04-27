using System;
using System.Threading.Tasks;
using Brickweave.Messaging.Serialization;
using Brickweave.Messaging.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Brickweave.Messaging.SqlServer
{
    public class SqlServerMessageFailureWriter<TContext> : IMessageFailureHandler
        where TContext : DbContext, IMessageStore
    {
        private readonly TContext _dbContext;
        private readonly IMessageSerializer _serializer;

        public SqlServerMessageFailureWriter(TContext dbContext, IMessageSerializer serializer)
        {
            _dbContext = dbContext;
            _serializer = serializer;
        }

        public async Task Handle(IDomainEvent @event, Exception ex)
        {
            _dbContext.MessageFailures.Add(new MessageFailureData
            {
                Id = Guid.NewGuid(),
                Message = _serializer.SerializeObject(@event),
                Exception = ex.Message,
                Enqueued = DateTime.Now
            });

            await _dbContext.SaveChangesAsync();
        }
    }
}
