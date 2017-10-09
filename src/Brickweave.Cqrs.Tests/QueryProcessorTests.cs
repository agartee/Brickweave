using System.Threading.Tasks;
using FluentAssertions;
using Brickweave.Cqrs.Exceptions;
using Brickweave.Cqrs.Tests.Fakes;
using NSubstitute;
using Xunit;

// ReSharper disable PossibleNullReferenceException

namespace Brickweave.Cqrs.Tests
{
    public class QueryProcessorTests
    {
        [Fact]
        public void ProcessAsync_WithNoRegisteredQueryHandler_Throws()
        {
            var serviceLocator = Substitute.For<IServiceLocator>();
            var queryProcessor = new QueryProcessor(serviceLocator);

            var exception = Record.ExceptionAsync(async () => await queryProcessor.ProcessAsync(new TestQuery("1")));

            exception.Result.Should().BeOfType<QueryHandlerNotRegisteredException>();
            exception.Result.Message.Should().Be($"Handler not registered for query, {typeof(TestQuery)}");
        }

        [Fact]
        public void ProcessAsync_WithNullQuery_Throws()
        {
            var serviceLocator = Substitute.For<IServiceLocator>();
            var queryProcessor = new QueryProcessor(serviceLocator);

            var exception = Record.ExceptionAsync(async () => await queryProcessor.ProcessAsync(null));

            exception.Result.Should().BeOfType<GuardException>();
            exception.Result.Message.Should().Be("Unable to process null query.");
        }

        [Fact]
        public async Task ProcessAsync_WithRegisteredHandler_ExecutesHandler()
        {
            var handler = new TestQueryHandler();

            var serviceLocator = Substitute.For<IServiceLocator>();
            serviceLocator.GetInstance(typeof(IQueryHandler<TestQuery, Result>))
                .Returns(handler);

            var queryProcessor = new QueryProcessor(serviceLocator);
            var result = await queryProcessor.ProcessAsync(new TestQuery("1"));

            result.Should().Be(new Result("1"));
        }
    }
}
