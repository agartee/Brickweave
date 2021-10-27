﻿using System;
using System.Threading.Tasks;
using Brickweave.Cqrs.Cli.Models;
using Brickweave.Cqrs.Cli.SqlServer.Services;
using Brickweave.Cqrs.Cli.SqlServer.Tests.Data;
using Brickweave.Cqrs.Cli.SqlServer.Tests.Fixtures;
using Brickweave.Cqrs.Cli.SqlServer.Tests.Models;
using Brickweave.Serialization;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Brickweave.Cqrs.Cli.SqlServer.Tests.Services
{
    [Trait("Category", "Integration")]
    [Collection("SqlServerTestCollection")]
    public class SqlServerCommandQueueTests
    {
        [Fact]
        public async Task EnqueueCommandAsync_SavesCommand()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task GetNext_WithQueuedCommand_ReturnsNextQueuedCommand()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task GetNext_WithNoQueuedCommands_ReturnsNull()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task GetNext_WithQueuedCommandThatIsFlaggedAsProcessing_ReturnsNextQueuedCommandThatIsNotProcessing()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task Delete_WhenCommandExists_DeletesCommand()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task Delete_WhenCommandDoesNotExist_DoesNotThrow()
        {
            throw new NotImplementedException();
        }
    }
}
