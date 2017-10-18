using System.Security.Claims;
using System.Threading.Tasks;

namespace Brickweave.Cqrs.Tests.Models
{
    public class TestSecuredCommandWithResultHandler : ISecuredCommandHandler<TestCommandWithResult, Result>
    {
        public async Task<Result> HandleAsync(TestCommandWithResult command, ClaimsPrincipal user)
        {
            return new Result(command.Value);
        }
    }
}