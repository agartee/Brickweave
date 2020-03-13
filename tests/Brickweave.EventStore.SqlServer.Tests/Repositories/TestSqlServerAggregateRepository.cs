using System;
using System.Threading.Tasks;
using Brickweave.EventStore.Factories;
using Brickweave.EventStore.SqlServer.Tests.Data;
using Brickweave.EventStore.SqlServer.Tests.Models;
using Brickweave.Serialization;

namespace Brickweave.EventStore.SqlServer.Tests.Repositories
{
    public class TestSqlServerAggregateRepository : AggregateRepository<TestAggregate>
    {
        private readonly EventStoreDbContext _dbContext;

        public TestSqlServerAggregateRepository(EventStoreDbContext dbContext, IDocumentSerializer serializer, 
            IAggregateFactory aggregateFactory) : base(serializer, aggregateFactory)
        {
            _dbContext = dbContext;
        }

        public async Task SaveTestAggregate(TestAggregate testAggregate)
        {
            AddUncommittedEvents(_dbContext.Events, testAggregate, testAggregate.Id);
            await _dbContext.SaveChangesAsync();

            testAggregate.ClearUncommittedEvents();
        }

        public async Task<TestAggregate> GetTestAggregate(Guid id)
        {
            return await CreateFromEventsAsync(_dbContext.Events, id);
        }
    }
}
