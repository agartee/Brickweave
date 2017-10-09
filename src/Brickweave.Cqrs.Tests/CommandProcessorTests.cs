using System;
using System.Threading.Tasks;
using Brickweave.Core.Exceptions;
using Brickweave.Cqrs.Exceptions;
using Brickweave.Cqrs.Tests.Fakes;
using FluentAssertions;
using NSubstitute;
using Xunit;

// ReSharper disable PossibleNullReferenceException

namespace Brickweave.Cqrs.Tests
{
    public class CommandProcessorTests
    {
        [Fact]
        public void ProcessAsync_WithNoRegisteredCommandHandler_Throws()
        {
            var serviceLocator = Substitute.For<IServiceProvider>();
            var commandProcessor = new CommandProcessor(serviceLocator);

            var exception = Record.ExceptionAsync(async () => await commandProcessor.ProcessAsync(new TestCommand()));

            exception.Result.Should().BeOfType<CommandHandlerNotRegisteredException>();
            exception.Result.Message.Should().Be($"Handler not registered for command, {typeof(TestCommand)}");
        }

        [Fact]
        public void ProcessAsync_WithNullCommand_Throws()
        {
            var serviceLocator = Substitute.For<IServiceProvider>();
            var commandProcessor = new CommandProcessor(serviceLocator);

            var exception = Record.ExceptionAsync(async () => await commandProcessor.ProcessAsync(null));

            exception.Result.Should().BeOfType<GuardException>();
            exception.Result.Message.Should().Be("Unable to process null command.");
        }

        [Fact]
        public async Task ProcessAsync_WithRegisteredHandler_ExecutesHandler()
        {
            var handler = new TestCommandWithResultHandler();

            var serviceLocator = Substitute.For<IServiceProvider>();
            serviceLocator.GetService(typeof(ICommandHandler<TestCommandWithResult, Result>))
                .Returns(handler);

            var commandProcessor = new CommandProcessor(serviceLocator);
            var result = await commandProcessor.ProcessAsync(new TestCommandWithResult("1"));

            result.Should().Be(new Result("1"));
        }

        [Fact]
        public async Task ProcessAsync_WithRegisteredHandlerAndNoResultReturned_ExecutesHandler()
        {
            var handlerWasCalled = false;
            var handler = new TestCommandHandler(() => handlerWasCalled = true);

            var serviceLocator = Substitute.For<IServiceProvider>();
            serviceLocator.GetService(typeof(ICommandHandler<TestCommand>))
                .Returns(handler);

            var commandProcessor = new CommandProcessor(serviceLocator);
            await commandProcessor.ProcessAsync(new TestCommand());

            handlerWasCalled.Should().BeTrue();
        }

    }
}
