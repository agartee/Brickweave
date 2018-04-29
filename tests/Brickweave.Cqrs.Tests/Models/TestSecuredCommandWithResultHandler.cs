using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Brickweave.Cqrs.Tests.Models
{
    public class TestSecuredCommandWithResultHandler : ISecuredCommandHandler<TestCommandWithResult, Result>
    {
        public Task<Result> HandleAsync(TestCommandWithResult command, ClaimsPrincipal user, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(new Result(command.Value));
        }
    }
}