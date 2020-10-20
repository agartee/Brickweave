using System;
using System.Linq;
using System.Threading.Tasks;
using Brickweave.EventStore.Factories;
using Brickweave.EventStore.SqlServer.Tests.Fixtures;
using Brickweave.EventStore.SqlServer.Tests.Models;
using Brickweave.Serialization;
using FluentAssertions;
using Xunit;

namespace Brickweave.EventStore.SqlServer.Tests.Repositories
{
    [Trait("Category", "Integration")]
    [Collection("SqlServerTestCollection")]
    public class SqlServerTestAggregateRepositoryTests
    {
        private readonly SqlServerFixture _fixture;
        private readonly SqlServerTestAggregateRepository _repository;

        public SqlServerTestAggregateRepositoryTests(SqlServerFixture fixture)
        {
            _fixture = fixture;
            _fixture.ClearData();

            _repository = new SqlServerTestAggregateRepository(
                _fixture.CreateDbContext(),
                new JsonDocumentSerializer(typeof(TestAggregateCreated)),
                new ReflectionConventionAggregateFactory());
        }

        [Fact]
        public async Task SaveTestAggregate_CreatesEvents()
        {
            var id = new Guid("a48fc8b1-bc2f-436f-9717-1717949d9b14");
            var testAggregate = new TestAggregate(id);

            await _repository.SaveTestAggregate(testAggregate);

            var dbContext = _fixture.CreateDbContext();
            var events = dbContext.Events.ToList();

            events.Should().HaveCount(1);

            var @event = events.First();
            @event.StreamId.Should().Be(id);
            @event.TypeName.Should().Be(nameof(TestAggregateCreated));
            @event.Json.Should().Be("{\"id\":\"a48fc8b1-bc2f-436f-9717-1717949d9b14\"}");
        }

        [Fact]
        public async Task GetTestAggregate_ReturnsTestAggregate()
        {
            var id = Guid.NewGuid();
            var testAggregate = new TestAggregate(id);
            await _repository.SaveTestAggregate(testAggregate);

            var result = await _repository.GetTestAggregate(id);

            result.Id.Should().Be(id);
        }

        [Fact]
        public async Task GetTestAggregateList_ReturnsTestAggregateList()
        {
            var id1 = Guid.NewGuid();
            var testAggregate1 = new TestAggregate(id1);
            await _repository.SaveTestAggregate(testAggregate1);

            var id2 = Guid.NewGuid();
            var testAggregate2 = new TestAggregate(id2);
            await _repository.SaveTestAggregate(testAggregate2);

            var results = await _repository.GetTestAggregateList(new[] { id1, id2 });

            results.Should().HaveCount(2);

            results.Where(a => a.Id.Equals(id1)).Should().HaveCount(1);
            results.Where(a => a.Id.Equals(id2)).Should().HaveCount(1);
        }
    }
}
