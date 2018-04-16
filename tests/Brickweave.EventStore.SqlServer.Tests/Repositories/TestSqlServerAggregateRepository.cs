using System;
using System.Threading.Tasks;
using Brickweave.EventStore.Factories;
using Brickweave.EventStore.Serialization;
using Brickweave.EventStore.SqlServer.Tests.Models;

namespace Brickweave.EventStore.SqlServer.Tests.Repositories
{
    public class TestSqlServerAggregateRepository : SqlServerAggregateRepository<TestAggregate, EventStoreDbContext>
    {
        public TestSqlServerAggregateRepository(EventStoreDbContext dbContext, IDocumentSerializer serializer, 
            IAggregateFactory aggregateFactory) : base(dbContext, serializer, aggregateFactory)
        {
            
        }

        public async Task SaveTestAggregate(TestAggregate testAggregate)
        {
            await SaveUncommittedEventsAsync(testAggregate, testAggregate.TestId);
        }

        public async Task<TestAggregate> GetTestAggregate(Guid testId)
        {
            return await TryFindAsync(testId);
        }

        public async Task DeleteTestAggregate(Guid testId)
        {
            await DeleteAsync(testId);
        }
    }
}
