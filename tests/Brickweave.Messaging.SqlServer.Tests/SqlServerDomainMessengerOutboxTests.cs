using System;
using System.Threading.Tasks;
using Brickweave.Messaging.Serialization;
using Brickweave.Messaging.SqlServer.Entities;
using Brickweave.Messaging.SqlServer.Tests.Data;
using Brickweave.Messaging.SqlServer.Tests.Fixtures;
using Xunit;

namespace Brickweave.Messaging.SqlServer.Tests
{
    public class SqlServerDomainMessengerOutboxTests
    {
        private readonly SqlServerFixture _fixture;

        public SqlServerDomainMessengerOutboxTests(SqlServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task EnqueueAsync_SavesEventsToDatabase()
        {
            var outbox = new SqlServerDomainMessengerOutbox<MessageStoreDbContext, MessageData>(
                new JsonMessageSerializer(),
                _fixture.DbContext,
                dbContext => dbContext.MessageOutbox);

            throw new NotImplementedException();
        }
    }
}
