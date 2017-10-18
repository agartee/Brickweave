using System;
using System.Threading.Tasks;
using Brickweave.Cqrs.Exceptions;
using Brickweave.Cqrs.Tests.Models;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Brickweave.Cqrs.Tests
{
    public class QueryExecutorTests
    {
        [Fact]
        public async Task ExecuteAsync_WhenQueryHandlerIsNotRegistered_Throws()
        {
            var serviceLocator = Substitute.For<IServiceProvider>();
            var queryProcessor = new QueryExecutor(serviceLocator);

            var exception = await Assert.ThrowsAsync<QueryHandlerNotRegisteredException>(
                () => queryProcessor.ExecuteAsync(new TestQuery("1")));

            exception.Should().NotBeNull();
            exception.Query.Should().BeOfType<TestQuery>();
        }

        [Fact]
        public async Task ExecuteAsync_WhenQueryIsNull_Throws()
        {
            var serviceLocator = Substitute.For<IServiceProvider>();
            var queryProcessor = new QueryExecutor(serviceLocator);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => queryProcessor.ExecuteAsync(null));

            exception.Should().NotBeNull();
        }

        [Fact]
        public async Task ExecuteAsync_WhenHandlerIsRegistered_ExecutesHandler()
        {
            var handler = new TestQueryHandler();

            var serviceLocator = Substitute.For<IServiceProvider>();
            serviceLocator.GetService(typeof(IQueryHandler<TestQuery, Result>))
                .Returns(handler);

            var queryProcessor = new QueryExecutor(serviceLocator);
            var result = await queryProcessor.ExecuteAsync(new TestQuery("1"));

            result.Should().Be(new Result("1"));
        }

        [Fact]
        public async Task ExecuteAsync_WhenSecuredHandlerIsRegistered_ExecutesHandler()
        {
            var handler = new TestSecuredQueryHandler();

            var serviceLocator = Substitute.For<IServiceProvider>();
            serviceLocator.GetService(typeof(IQueryHandler<TestQuery, Result>))
                .Returns(handler);

            var queryProcessor = new QueryExecutor(serviceLocator);
            var result = await queryProcessor.ExecuteAsync(new TestQuery("1"));

            result.Should().Be(new Result("1"));
        }
    }
}
