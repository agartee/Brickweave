using System;
using System.Threading;
using System.Threading.Tasks;
using Brickweave.Cqrs.Exceptions;
using Brickweave.Cqrs.Tests.Models;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Brickweave.Cqrs.Tests
{
    public class CommandDispatcherTests
    {
        [Fact]
        public async Task ExecuteAsync_WhenCommandHandlerIsNotRegistered_Throws()
        {
            var serviceProvider = Substitute.For<IServiceProvider>();
            var commandExecutor = new CommandDispatcher(serviceProvider);
            
            var exception = await Assert.ThrowsAsync<CommandHandlerNotRegisteredException>(
                () => commandExecutor.ExecuteAsync(new TestCommand()));

            exception.Command.Should().BeOfType<TestCommand>();
        }

        [Fact]
        public async Task ExecuteAsync_WhenCommandIsNull_Throws()
        {
            var serviceProvider = Substitute.For<IServiceProvider>();
            var commandExecutor = new CommandDispatcher(serviceProvider);

            await Assert.ThrowsAsync<ArgumentNullException>(
                () => commandExecutor.ExecuteAsync(null));
            }

        [Fact]
        public async Task ExecuteAsync_WhenHandlerIsRegistered_ExecutesHandler()
        {
            var handlerWasCalled = false;
            var handler = new TestCommandHandler(() => handlerWasCalled = true, null);

            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(ICommandHandler<TestCommand>))
                .Returns(handler);

            var commandExecutor = new CommandDispatcher(serviceProvider);
            await commandExecutor.ExecuteAsync(new TestCommand());

            handlerWasCalled.Should().BeTrue();
        }

        [Fact]
        public async Task ExecuteAsync_WhenHandlerIsRegistered_ExecutesHandlerWithCancellationRequest()
        {
            var handlerWasCalledWithCancellationRequest = false;
            var handler = new TestCommandHandler(null, () => handlerWasCalledWithCancellationRequest = true);

            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(ICommandHandler<TestCommand>))
                .Returns(handler);

            // Simulate cancellation for the command handler.
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            var commandExecutor = new CommandDispatcher(serviceProvider);
            await commandExecutor.ExecuteAsync(new TestCommand(), cancellationToken: cancellationTokenSource.Token);

            handlerWasCalledWithCancellationRequest.Should().BeTrue();
        }

        [Fact]
        public async Task ExecuteAsync_WhenSecuredHandlerIsRegistered_ExecutesHandler()
        {
            var handlerWasCalled = false;
            var handler = new TestSecuredCommandHandler(() => handlerWasCalled = true, null);

            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(ISecuredCommandHandler<TestCommand>))
                .Returns(handler);

            var commandExecutor = new CommandDispatcher(serviceProvider);
            await commandExecutor.ExecuteAsync(new TestCommand());

            handlerWasCalled.Should().BeTrue();
        }

        [Fact]
        public async Task ExecuteAsync_WhenSecuredHandlerIsRegistered_ExecutesHandlerWithCancellationRequest()
        {
            var handlerWasCalledWithCancellationRequest = false;
            var handler = new TestSecuredCommandHandler(null, () => handlerWasCalledWithCancellationRequest = true);

            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(ISecuredCommandHandler<TestCommand>))
                .Returns(handler);

            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            var commandExecutor = new CommandDispatcher(serviceProvider);
            await commandExecutor.ExecuteAsync(new TestCommand(), cancellationToken: cancellationTokenSource.Token);

            handlerWasCalledWithCancellationRequest.Should().BeTrue();
        }

        [Fact]
        public async Task ExecuteAsync_WhenCommandHandlerIsRegistered_ReturnsResult()
        {
            var handler = new TestCommandWithResultHandler();

            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(ICommandHandler<TestCommandWithResult, Result>))
                .Returns(handler);

            var commandExecutor = new CommandDispatcher(serviceProvider);
            var result = await commandExecutor.ExecuteAsync(new TestCommandWithResult("1"));

            result.Should().Be(new Result("1"));
        }

        [Fact]
        public async Task ExecuteAsync_WhenSecuredCommandHandlerIsRegistered_ReturnsResult()
        {
            var handler = new TestSecuredCommandWithResultHandler();

            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(ISecuredCommandHandler<TestCommandWithResult, Result>))
                .Returns(handler);

            var commandExecutor = new CommandDispatcher(serviceProvider);
            var result = await commandExecutor.ExecuteAsync(new TestCommandWithResult("1"));

            result.Should().Be(new Result("1"));
        }
    }
}
