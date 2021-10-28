﻿using System;
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
    public class SqlServerCommandStatusRepositoryTests
    {
        [Trait("Category", "Integration")]
        [Collection("SqlServerTestCollection")]
        public class CommandStatusRepositoryTests
        {
            private readonly SqlServerFixture _fixture;
            private readonly SqlServerCommandStatusRepository<CqrsDbContext> _repository;

            public CommandStatusRepositoryTests(SqlServerFixture fixture)
            {
                _fixture = fixture;
                _fixture.ClearData();

                _repository = new SqlServerCommandStatusRepository<CqrsDbContext>(
                    _fixture.CreateDbContext(),
                    dbContext => dbContext.CommandQueue,
                    dbContext => dbContext.CommandStatus,
                    new JsonDocumentSerializer());
            }

            [Fact]
            public async Task ReportStatusAsync_WhenStatusIsStarted_WritesStatus()
            {
                var id = Guid.NewGuid();

                await _repository.ReportStartedAsync(id);

                var dbContext = _fixture.CreateDbContext();

                var data = await dbContext.CommandStatus
                    .FirstOrDefaultAsync(s => s.Id == id);

                data.Should().NotBeNull();
            }

            [Fact]
            public async Task ReportCompletedAsync_WhenStatusExistsAndIsStarted_WritesStatus()
            {
                var id = Guid.NewGuid();

                await _repository.ReportStartedAsync(id);
                await _repository.ReportCompletedAsync(id, new TestCommandResult("test"));

                var dbContext = _fixture.CreateDbContext();

                var data = await dbContext.CommandStatus
                    .FirstOrDefaultAsync(s => s.Id == id);

                data.Should().NotBeNull();
            }

            [Fact]
            public async Task ReportErrorAsync_WhenStatusExistsAndIsStarted_WritesStatus()
            {
                var id = Guid.NewGuid();

                await _repository.ReportStartedAsync(id);
                await _repository.ReportErrorAsync(id, new InvalidOperationException("Unable to comply."));

                var dbContext = _fixture.CreateDbContext();

                var data = await dbContext.CommandStatus
                    .FirstOrDefaultAsync(s => s.Id == id);

                data.Should().NotBeNull();
            }

            [Fact]
            public async Task ReadStatusAsync_WhenStatusIsRunning_ReturnsStatus()
            {
                var id = Guid.NewGuid();

                await _repository.ReportStartedAsync(id);

                var result = await _repository.ReadStatusAsync(id);

                result.Should().BeOfType<RunningExecutionStatus>();
            }

            [Fact]
            public async Task ReadStatusAsync_WhenStatusIsCompleted_ReturnsStatus()
            {
                var id = Guid.NewGuid();

                await _repository.ReportStartedAsync(id);
                await _repository.ReportCompletedAsync(id, new TestCommandResult("test"));

                var result = (await _repository.ReadStatusAsync(id)).As<CompletedExecutionStatus>();

                result.Should().NotBeNull();
                result.Result.Should().BeOfType<TestCommandResult>();
            }

            [Fact]
            public async Task ReadStatusAsync_WhenStatusIsError_ReturnsStatus()
            {
                var id = Guid.NewGuid();

                await _repository.ReportStartedAsync(id);
                await _repository.ReportErrorAsync(id, new InvalidOperationException("Unable to comply."));

                var result = (await _repository.ReadStatusAsync(id)).As<ErrorExecutionStatus>();

                result.Should().NotBeNull();
                result.Message.Should().Be("Unable to comply.");
            }
        }
    }
}
