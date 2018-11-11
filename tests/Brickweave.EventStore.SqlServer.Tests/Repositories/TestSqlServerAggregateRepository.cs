using System;
using System.Threading.Tasks;
using Brickweave.EventStore.Factories;
using Brickweave.EventStore.Serialization;
using Brickweave.EventStore.SqlServer.Tests.Models;

namespace Brickweave.EventStore.SqlServer.Tests.Repositories
{
    public class TestSqlServerAggregateRepository : SqlServerAggregateRepository<TestAggregate, EventStoreDbContext>
    {
        private readonly EventStoreDbContext _dbContext;

        public TestSqlServerAggregateRepository(EventStoreDbContext dbContext, IDocumentSerializer serializer, 
            IAggregateFactory aggregateFactory) : base(dbContext.Events, serializer, aggregateFactory)
        {
            _dbContext = dbContext;
        }

        public async Task SaveTestAggregate(TestAggregate testAggregate)
        {
            AddUncommittedEvents(testAggregate, testAggregate.TestId);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<TestAggregate> GetTestAggregate(Guid testId)
        {
            return await GetFromEventsAsync(testId);
        }

        public async Task DeleteTestAggregate(Guid testId)
        {
            await RemoveEventsAsync(testId);
            await _dbContext.SaveChangesAsync();
        }
    }
}
