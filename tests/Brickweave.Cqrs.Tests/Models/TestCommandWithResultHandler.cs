using System.Threading;
using System.Threading.Tasks;

namespace Brickweave.Cqrs.Tests.Models
{
    public class TestCommandWithResultHandler : ICommandHandler<TestCommandWithResult, Result>
    {
        public Task<Result> HandleAsync(TestCommandWithResult command, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(new Result(command.Value));
        }
    }
}