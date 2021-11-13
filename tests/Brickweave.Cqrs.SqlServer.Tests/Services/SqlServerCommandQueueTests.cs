using System;
using System.Threading.Tasks;
using Brickweave.Cqrs.Models;
using Brickweave.Cqrs.SqlServer.Services;
using Brickweave.Cqrs.SqlServer.Tests.Data;
using Brickweave.Cqrs.SqlServer.Tests.Fixtures;
using Brickweave.Cqrs.SqlServer.Tests.Models;
using Brickweave.Serialization;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Brickweave.Cqrs.SqlServer.Tests.Services
{
    [Trait("Category", "Integration")]
    [Collection("SqlServerTestCollection")]
    public class SqlServerCommandQueueTests
    {
        private readonly SqlServerFixture _fixture;
        private readonly SqlServerCommandQueue<CqrsDbContext> _commandQueue;

        public SqlServerCommandQueueTests(SqlServerFixture fixture)
        {
            _fixture = fixture;
            _fixture.ClearData();

            _commandQueue = new SqlServerCommandQueue<CqrsDbContext>(
                _fixture.CreateDbContext(),
                dbContext => dbContext.CommandQueue,
                new JsonDocumentSerializer());
        }

        [Fact]
        public async Task EnqueueCommandAsync_SavesCommand()
        {
            var commandId = Guid.NewGuid();
            var command = new TestCommand("test");

            await _commandQueue.EnqueueCommandAsync(commandId, command);

            var result = await _fixture.CreateDbContext().CommandQueue
                .FirstOrDefaultAsync(c => c.Id == commandId);

            result.CommandTypeName.Should().Be(typeof(TestCommand).AssemblyQualifiedName);
        }

        [Fact]
        public async Task GetNext_WithQueuedCommand_ReturnsNextQueuedCommand()
        {
            var commandId = Guid.NewGuid();
            var command = new TestCommand("test");

            await _commandQueue.EnqueueCommandAsync(commandId, command);

            var result = await _commandQueue.GetNextAsync();

            result.Value.Should().BeOfType(typeof(TestCommand));
            result.Value.As<TestCommand>().Foo.Should().Be("test");
        }

        [Fact]
        public async Task GetNext_WithNoQueuedCommands_ReturnsNull()
        {
            var result = await _commandQueue.GetNextAsync();

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetNext_WithQueuedCommandThatIsFlaggedAsStarted_ReturnsNextQueuedCommandThatIsNotProcessing()
        {
            var command1Id = Guid.NewGuid();
            var command1 = new TestCommand("test1");
            var command2Id = Guid.NewGuid();
            var command2 = new TestCommand("test2");

            await _commandQueue.EnqueueCommandAsync(command1Id, command1);
            await _commandQueue.EnqueueCommandAsync(command2Id, command2);
            
            await _commandQueue.GetNextAsync(); // test1
            var result = await _commandQueue.GetNextAsync(); // test2

            result.Value.Should().BeOfType(typeof(TestCommand));
            result.Value.As<TestCommand>().Foo.Should().Be("test2");
        }

        [Fact]
        public async Task ReportCompletedAsync_WhenCommandExists_UpdatesCommand()
        {
            var commandId = Guid.NewGuid();
            var command = new TestCommand("test");

            await _commandQueue.EnqueueCommandAsync(commandId, command);
            await _commandQueue.GetNextAsync();

            await _commandQueue.ReportCompletedAsync(commandId, new TestCommandResult("result"));

            var result = await _fixture.CreateDbContext().CommandQueue
                .FirstOrDefaultAsync(c => c.Id == commandId);

            result.Completed.Should().NotBeNull();
            result.ResultTypeName.Should().Be(typeof(TestCommandResult).AssemblyQualifiedName);
            result.ResultJson.Should().Contain("result");
        }

        [Fact]
        public async Task ReportCompletedAsync_WhenCommandDoesNotExist_Throws()
        {
            var commandId = Guid.NewGuid();
            
            var exception = await Record.ExceptionAsync(() => 
                _commandQueue.ReportCompletedAsync(commandId, new TestCommandResult("result")));

            exception.Should().BeOfType<InvalidOperationException>();
        }

        [Fact]
        public async Task ReportExceptionAsync_WhenCommandExists_UpdatesCommand()
        {
            var commandId = Guid.NewGuid();
            var command = new TestCommand("test");

            await _commandQueue.EnqueueCommandAsync(commandId, command);
            await _commandQueue.GetNextAsync();

            await _commandQueue.ReportExceptionAsync(commandId, new InvalidOperationException("boo"));

            var result = await _fixture.CreateDbContext().CommandQueue
                .FirstOrDefaultAsync(c => c.Id == commandId);

            result.Completed.Should().NotBeNull();
            result.ResultTypeName.Should().Be(typeof(ExceptionInfo).AssemblyQualifiedName);
            result.ResultJson.Should().Contain(nameof(InvalidOperationException));
            result.ResultJson.Should().Contain("boo");
        }

        [Fact]
        public async Task ReportExceptionAsync_WhenCommandDoesNotExist_Throws()
        {
            var commandId = Guid.NewGuid();

            var exception = await Record.ExceptionAsync(() =>
                _commandQueue.ReportExceptionAsync(commandId, new InvalidOperationException("boo")));

            exception.Should().BeOfType<InvalidOperationException>();
        }

        [Fact]
        public async Task Delete_WhenCommandExists_DeletesCommand()
        {
            var commandId = Guid.NewGuid();
            var command = new TestCommand("test");

            await _commandQueue.EnqueueCommandAsync(commandId, command);

            await _commandQueue.DeleteAsync(commandId);

            var result = await _fixture.CreateDbContext().CommandQueue
                .FirstOrDefaultAsync(c => c.Id == commandId);

            result.Should().BeNull();
        }

        [Fact]
        public async Task Delete_WhenCommandDoesNotExist_DoesNotThrow()
        {
            var commandId = Guid.NewGuid();

            var exception = await Record.ExceptionAsync(() => 
                _commandQueue.DeleteAsync(commandId));

            exception.Should().BeNull();
        }
    }
}
