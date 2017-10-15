using System;
using System.Threading.Tasks;
using Brickweave.Cqrs.Exceptions;
using Brickweave.Cqrs.Tests.Models;
using FluentAssertions;
using NSubstitute;
using Xunit;

// ReSharper disable PossibleNullReferenceException

namespace Brickweave.Cqrs.Tests
{
    public class QueryExecutorTests
    {
        [Fact]
        public void ExecuteAsync_WhenQueryHandlerIsNotRegistered_Throws()
        {
            var serviceLocator = Substitute.For<IServiceProvider>();
            var queryProcessor = new QueryExecutor(serviceLocator);

            var exception = Record.ExceptionAsync(async () => await queryProcessor.ExecuteAsync(new TestQuery("1")));

            exception.Result.Should().BeOfType<QueryHandlerNotRegisteredException>();
            exception.Result.Message.Should().Be($"Handler not registered for query, {typeof(TestQuery)}");
        }

        [Fact]
        public void ExecuteAsync_WhenQueryIsNull_Throws()
        {
            var serviceLocator = Substitute.For<IServiceProvider>();
            var queryProcessor = new QueryExecutor(serviceLocator);

            var exception = Record.ExceptionAsync(async () => await queryProcessor.ExecuteAsync(null));

            exception.Result.Should().BeOfType<ArgumentNullException>();
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
    }
}
