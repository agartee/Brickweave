using System;
using System.Threading.Tasks;
using Brickweave.Cqrs.Cli.Factories;
using Brickweave.Cqrs.Cli.Models;
using Brickweave.Cqrs.Cli.Parsers;
using Brickweave.Cqrs.Cli.Tests.Models;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Brickweave.Cqrs.Cli.Tests
{
    public class RunnerTests
    {
        [Fact]
        public async Task Run_WhenCommandIsDetermined_ReturnsCommandHandlerResult()
        {
            var executableFactory = Substitute.For<IExecutableFactory>();
            executableFactory.Create(Arg.Any<ExecutableInfo>())
                .Returns(new CreateFoo(0, DateTime.MinValue));

            var commandExecutor = Substitute.For<ICommandExecutor>();
            commandExecutor.ExecuteAsync(Arg.Any<ICommand>()).Returns("success!");

            var runner = new Runner(
                Substitute.For<IArgParser>(),
                executableFactory,
                commandExecutor,
                Substitute.For<IQueryExecutor>());

            var result = await runner.Run(new [] {string.Empty});

            result.Should().Be("success!");
            await commandExecutor.Received(1).ExecuteAsync(Arg.Any<ICommand>());
        }

        [Fact]
        public async Task Run_WhenQueryIsDetermined_ReturnsQueryHandlerResult()
        {
            var executableFactory = Substitute.For<IExecutableFactory>();
            executableFactory.Create(Arg.Any<ExecutableInfo>())
                .Returns(new GetFoo());

            var queryExecutor = Substitute.For<IQueryExecutor>();
            queryExecutor.ExecuteAsync(Arg.Any<IQuery>()).Returns("success!");

            var runner = new Runner(
                Substitute.For<IArgParser>(),
                executableFactory,
                Substitute.For<ICommandExecutor>(),
                queryExecutor);

            var result = await runner.Run(new[] { string.Empty });

            result.Should().Be("success!");
            await queryExecutor.Received(1).ExecuteAsync(Arg.Any<IQuery>());
        }
    }

    
}
