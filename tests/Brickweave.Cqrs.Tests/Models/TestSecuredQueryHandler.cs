using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Brickweave.Cqrs.Tests.Models
{
    public class TestSecuredQueryHandler : ISecuredQueryHandler<TestQuery, Result>
    {
        public Task<Result> HandleAsync(TestQuery query, ClaimsPrincipal principal, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(new Result(query.Value));
        }
    }
}