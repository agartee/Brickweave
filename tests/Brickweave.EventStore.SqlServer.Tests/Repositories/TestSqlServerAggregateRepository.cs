using System;
using System.Threading.Tasks;
using Brickweave.EventStore.SqlServer.Extensions;
using Brickweave.EventStore.SqlServer.Tests.Models;

namespace Brickweave.EventStore.SqlServer.Tests.Repositories
{
    public class TestSqlServerAggregateRepository
    {
        private readonly EventStoreDbContext _dbContext;

        public TestSqlServerAggregateRepository(EventStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveTestAggregate(TestAggregate testAggregate)
        {
            _dbContext.Events.AddUncommittedEvents(testAggregate, testAggregate.TestId);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<TestAggregate> GetTestAggregate(Guid testId)
        {
            return await _dbContext.Events.CreateFromEventsAsync<TestAggregate>(testId);
        }

        public async Task DeleteTestAggregate(Guid testId)
        {
            await _dbContext.Events.RemoveEventsAsync(testId);
            await _dbContext.SaveChangesAsync();
        }
    }
}
