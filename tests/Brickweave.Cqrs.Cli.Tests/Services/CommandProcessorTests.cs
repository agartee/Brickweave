using System;
using System.Threading.Tasks;
using Xunit;

namespace Brickweave.Cqrs.Cli.Tests.Services
{
    public class CommandProcessorTests
    {
        [Fact]
        public async Task ProcessCommandsAsync_WhenCommandEnqueued_ReportsStartedStatus()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task ProcessCommandsAsync_WhenCommandEnqueuedCompletes_ReportsCompletedStatus()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task ProcessCommandsAsync_WhenNoCommandEnqueued_DoesNotThrow()
        {
            throw new NotImplementedException();
        }
        
        [Fact]
        public async Task ProcessCommandsAsync_WhenEnqueuedCommandThrows_ReportsErrorStatus()
        {
            throw new NotImplementedException();
        }
    }
}
