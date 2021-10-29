using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Brickweave.Cqrs.Cli.Factories;
using Brickweave.Cqrs.Cli.Models;
using Brickweave.Cqrs.Cli.Tests.Models;
using Brickweave.Cqrs.Services;
using FluentAssertions;
using NSubstitute;
using Xunit;
using Xunit.Abstractions;

namespace Brickweave.Cqrs.Cli.Services.Tests
{
    public class CliDispatcherTests
    {
        private readonly ITestOutputHelper _output;

        public CliDispatcherTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Run_WhenHelpArgExists_ReturnsHelpInfo()
        {
            var helpInfo = new HelpInfo("test", string.Empty, string.Empty, HelpInfoType.Category);

            var helpParser = Substitute.For<IHelpInfoFactory>();
            helpParser.Create(Arg.Any<string[]>())
                .Returns(helpInfo);

            var runner = new CliDispatcher(
                Substitute.For<IExecutableInfoFactory>(),
                helpParser,
                Substitute.For<IExecutableFactory>(),
                Substitute.For<ICommandDispatcher>(),
                Substitute.For<IQueryDispatcher>());

            var result = await runner.DispatchAsync("test --help");
            result.Should().NotBeNull();

            _output.WriteLine(result.ToString());
        }

        [Fact]
        public async Task Run_WhenCommandIsDetermined_ReturnsCommandHandlerResult()
        {
            var executableFactory = Substitute.For<IExecutableFactory>();
            executableFactory.Create(Arg.Any<ExecutableInfo>())
                .Returns(new CreateFoo(0, DateTime.MinValue));
            
            var commandDispatcher = Substitute.For<ICommandDispatcher>();
            commandDispatcher.ExecuteAsync(Arg.Any<ICommand>(), Arg.Any<ClaimsPrincipal>(), Arg.Any<Action<Guid>>()).Returns("success!");

            var runner = new CliDispatcher(
                Substitute.For<IExecutableInfoFactory>(),
                Substitute.For<IHelpInfoFactory>(),
                executableFactory,
                commandDispatcher,
                Substitute.For<IQueryDispatcher>());

            var result = await runner.DispatchAsync(string.Empty);

            result.Should().Be("success!");
            await commandDispatcher.Received(1).ExecuteAsync(Arg.Any<ICommand>(), Arg.Any<ClaimsPrincipal>(), Arg.Any<Action<Guid>>());
        }

        [Fact]
        public async Task Run_WhenQueryIsDetermined_ReturnsQueryHandlerResult()
        {
            var executableFactory = Substitute.For<IExecutableFactory>();
            executableFactory.Create(Arg.Any<ExecutableInfo>())
                .Returns(new GetFoo());

            var queryExecutor = Substitute.For<IQueryDispatcher>();
            queryExecutor.ExecuteAsync(Arg.Any<IQuery>(), Arg.Any<ClaimsPrincipal>()).Returns("success!");

            var runner = new CliDispatcher(
                Substitute.For<IExecutableInfoFactory>(),
                Substitute.For<IHelpInfoFactory>(),
                executableFactory,
                Substitute.For<ICommandDispatcher>(),
                queryExecutor);

            var result = await runner.DispatchAsync(string.Empty);

            result.Should().Be("success!");
            await queryExecutor.Received(1).ExecuteAsync(Arg.Any<IQuery>(), Arg.Any<ClaimsPrincipal>());
        }
    }

    
}
