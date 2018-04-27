using System;
using System.Linq;
using System.Threading.Tasks;
using Brickweave.EventStore.SqlServer.Tests.Fixtures;
using Brickweave.Messaging.Serialization;
using Brickweave.Messaging.SqlServer.Tests.Models;
using FluentAssertions;
using Xunit;

namespace Brickweave.Messaging.SqlServer.Tests
{
    [Trait("Category", "Integration")]
    public class SqlServerMessageFailureWriterTests : IClassFixture<SqlServerFixture>
    {
        private readonly SqlServerFixture _fixture;

        public SqlServerMessageFailureWriterTests(SqlServerFixture fixture)
        {
            _fixture = fixture;

            _fixture.ClearDatabase();
        }

        [Fact]
        public async Task Handle_SavesRow()
        {
            var id = Guid.NewGuid();
            var message = new TestDomainEvent(id);
            var exception = new Exception("error.");
            var testStarted = DateTime.Now;

            var handler = new SqlServerMessageFailureWriter<MessagingDbContext>(
                _fixture.DbContext, new JsonMessageSerializer());

            await handler.Handle(message, exception);

            var row = _fixture.DbContext.MessageFailures.First();
            
            row.Message.Should().Be($@"{{""id"":""{id}""}}");
            row.Exception.Should().Be("error.");
            row.Enqueued.Should().BeAfter(testStarted);
            row.Enqueued.Should().BeBefore(DateTime.Now);
        }
    }
}
