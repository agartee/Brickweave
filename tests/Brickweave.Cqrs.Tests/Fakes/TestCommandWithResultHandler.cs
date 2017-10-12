using System.Threading.Tasks;

namespace Brickweave.Cqrs.Tests.Fakes
{
    public class TestCommandWithResultHandler : ICommandHandler<TestCommandWithResult, Result>
    {
        public async Task<Result> HandleAsync(TestCommandWithResult command)
        {
            return new Result(command.Value);
        }
    }
}