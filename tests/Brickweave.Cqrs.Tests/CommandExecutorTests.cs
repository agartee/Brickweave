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
    public class CommandExecutorTests
    {
        [Fact]
        public void ExecuteAsync_WhenCommandHandlerIsNotRegistered_Throws()
        {
            var serviceLocator = Substitute.For<IServiceProvider>();
            var commandProcessor = new CommandExecutor(serviceLocator);

            var exception = Record.ExceptionAsync(async () => await commandProcessor.ExecuteAsync(new TestCommand()));

            exception.Result.Should().BeOfType<CommandHandlerNotRegisteredException>();
            exception.Result.Message.Should().Be($"Handler not registered for command, {typeof(TestCommand)}");
        }

        [Fact]
        public void ExecuteAsync_WhenCommandIsNull_Throws()
        {
            var serviceLocator = Substitute.For<IServiceProvider>();
            var commandProcessor = new CommandExecutor(serviceLocator);

            var exception = Record.ExceptionAsync(async () => await commandProcessor.ExecuteAsync(null));

            exception.Result.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public async Task ExecuteAsync_WhenHandlerIsRegistered_ExecutesHandler()
        {
            var handler = new TestCommandWithResultHandler();

            var serviceLocator = Substitute.For<IServiceProvider>();
            serviceLocator.GetService(typeof(ICommandHandler<TestCommandWithResult, Result>))
                .Returns(handler);

            var commandProcessor = new CommandExecutor(serviceLocator);
            var result = await commandProcessor.ExecuteAsync(new TestCommandWithResult("1"));

            result.Should().Be(new Result("1"));
        }

        [Fact]
        public async Task ExecuteAsync_WhenHandlerIsRegisteredAndReturnsNoResult_ExecutesHandler()
        {
            var handlerWasCalled = false;
            var handler = new TestCommandHandler(() => handlerWasCalled = true);

            var serviceLocator = Substitute.For<IServiceProvider>();
            serviceLocator.GetService(typeof(ICommandHandler<TestCommand>))
                .Returns(handler);

            var commandProcessor = new CommandExecutor(serviceLocator);
            await commandProcessor.ExecuteAsync(new TestCommand());

            handlerWasCalled.Should().BeTrue();
        }

    }
}
