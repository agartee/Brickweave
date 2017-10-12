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
    public class QueryProcessorTests
    {
        [Fact]
        public void ProcessAsync_WhenQueryHandlerIsNotRegistered_Throws()
        {
            var serviceLocator = Substitute.For<IServiceProvider>();
            var queryProcessor = new QueryProcessor(serviceLocator);

            var exception = Record.ExceptionAsync(async () => await queryProcessor.ProcessAsync(new TestQuery("1")));

            exception.Result.Should().BeOfType<QueryHandlerNotRegisteredException>();
            exception.Result.Message.Should().Be($"Handler not registered for query, {typeof(TestQuery)}");
        }

        [Fact]
        public void ProcessAsync_WhenQueryIsNull_Throws()
        {
            var serviceLocator = Substitute.For<IServiceProvider>();
            var queryProcessor = new QueryProcessor(serviceLocator);

            var exception = Record.ExceptionAsync(async () => await queryProcessor.ProcessAsync(null));

            exception.Result.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public async Task ProcessAsync_WhenHandlerIsRegistered_ExecutesHandler()
        {
            var handler = new TestQueryHandler();

            var serviceLocator = Substitute.For<IServiceProvider>();
            serviceLocator.GetService(typeof(IQueryHandler<TestQuery, Result>))
                .Returns(handler);

            var queryProcessor = new QueryProcessor(serviceLocator);
            var result = await queryProcessor.ProcessAsync(new TestQuery("1"));

            result.Should().Be(new Result("1"));
        }
    }
}
