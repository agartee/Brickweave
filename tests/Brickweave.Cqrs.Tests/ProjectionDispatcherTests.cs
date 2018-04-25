using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Brickweave.Cqrs.Tests.Models;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Brickweave.Cqrs.Tests
{
    public class ProjectionDispatcherTests
    {
        [Fact]
        public async Task ExecuteAsync_WhenProjectionHandlerIsNotRegistered_Throws()
        {
            var serviceProvider = Substitute.For<IServiceProvider>();
            var projectionExecutor = new ProjectionDispatcher(serviceProvider);
            
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => projectionExecutor.ExecuteAsync(new TestCommand()));

            exception.Message.Should().Contain(typeof(TestCommand).ToString());
        }

        [Fact]
        public async Task ExecuteAsync_WhenCommandIsNull_Throws()
        {
            var serviceProvider = Substitute.For<IServiceProvider>();
            var projectionExecutor = new ProjectionDispatcher(serviceProvider);

            await Assert.ThrowsAsync<ArgumentNullException>(
                () => projectionExecutor.ExecuteAsync(null));
            }

        [Fact]
        public async Task ExecuteAsync_WhenHandlerIsRegistered_ExecutesHandler()
        {
            var handlerWasCalled = false;
            var handler = new TestProjectionHandler(() => handlerWasCalled = true);

            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(Arg.Any<Type>()).Returns(new List<object>{handler});

            var projectionExecutor = new ProjectionDispatcher(serviceProvider);
            await projectionExecutor.ExecuteAsync(new TestCommand());

            handlerWasCalled.Should().BeTrue();
        }
    }
}
