using Brickweave.Cqrs.Attributes;
using Brickweave.Cqrs.Extensions;
using FluentAssertions;
using Xunit;

namespace Brickweave.Cqrs.Tests.Extensions
{
    public class CommandExtensionsTests
    {
        [Fact]
        public void IsLongRunningCommand_WhenAttributeDefined_ReturnsTrue()
        {
            var command = new LongRunningTestCommand();

            var result = command.IsLongRunning();

            result.Should().BeTrue();
        }

        [Fact]
        public void IsLongRunningCommand_WhenAttributeAbsent_ReturnsFalse()
        {
            var command = new TestCommand();

            var result = command.IsLongRunning();

            result.Should().BeFalse();
        }

        [LongRunningCommand]
        public class LongRunningTestCommand : ICommand
        {

        }

        public class TestCommand : ICommand
        {

        }
    }
}
