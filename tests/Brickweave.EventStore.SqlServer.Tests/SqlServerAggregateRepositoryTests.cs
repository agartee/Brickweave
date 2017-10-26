using System;
using System.Linq;
using System.Threading.Tasks;
using Brickweave.EventStore.Factories;
using Brickweave.EventStore.Serialization;
using Brickweave.EventStore.SqlServer.Tests.Fixtures;
using Brickweave.EventStore.SqlServer.Tests.Models;
using Brickweave.EventStore.SqlServer.Tests.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Brickweave.EventStore.SqlServer.Tests
{
    [Trait("Category", "Integration")]
    public class SqlServerAggregateRepositoryTests : IClassFixture<SqlServerFixture>
    {
        private readonly SqlServerFixture _fixture;
        private readonly TestSqlServerAggregateRepository _repository;
        
        public SqlServerAggregateRepositoryTests(SqlServerFixture fixture)
        {
            _fixture = fixture;
            _repository = new TestSqlServerAggregateRepository(
                _fixture.DbContext,
                new JsonDocumentSerializer(typeof(TestAggregateCreated)),
                new ReflectionConventionAggregateFactory());

            _fixture.ClearDatabase();
        }

        [Fact]
        public async Task SaveTestAggregate_SavesRows()
        {
            var testId = Guid.NewGuid();
            var aggregate = new TestAggregate(testId);

            await _repository.SaveTestAggregate(aggregate);

            var rows = (await _fixture.DbContext.Events
                .Where(e => e.StreamId == testId)
                .ToListAsync()).ToArray();

            rows.Should().HaveCount(1);

            var row = rows.First();
            row.StreamId.Should().Be(testId);
            row.CommitSequence.Should().Be(0);
        }

        [Fact]
        public async Task GetTestAggregate_ReturnsRow()
        {
            var testId = Guid.NewGuid();
            var savedTestAggregate = new TestAggregate(testId);
            await _repository.SaveTestAggregate(savedTestAggregate);

            var retrievedTestAggregate = await _repository.GetTestAggregate(testId);

            retrievedTestAggregate.TestId.Should().Be(testId);
        }

        [Fact]
        public async Task DeleteTestAggregate_DeletesRow()
        {
            var testId = Guid.NewGuid();
            var savedTestAggregate = new TestAggregate(testId);
            await _repository.SaveTestAggregate(savedTestAggregate);

            await _repository.DeleteTestAggregate(testId);

            var rows = await _fixture.DbContext.Events
                .Where(e => e.StreamId == testId)
                .ToListAsync();

            rows.Should().HaveCount(0);
        }
    }
}
