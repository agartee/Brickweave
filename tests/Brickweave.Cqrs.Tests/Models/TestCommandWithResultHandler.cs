using System.Threading.Tasks;

namespace Brickweave.Cqrs.Tests.Models
{
    public class TestCommandWithResultHandler : ICommandHandler<TestCommandWithResult, Result>
    {
        public Task<Result> HandleAsync(TestCommandWithResult command)
        {
            return Task.FromResult(new Result(command.Value));
        }
    }
}