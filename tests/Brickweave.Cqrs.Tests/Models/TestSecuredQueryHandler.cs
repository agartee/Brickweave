using System.Security.Claims;
using System.Threading.Tasks;

namespace Brickweave.Cqrs.Tests.Models
{
    public class TestSecuredQueryHandler : ISecuredQueryHandler<TestQuery, Result>
    {
        public Task<Result> HandleAsync(TestQuery query, ClaimsPrincipal principal)
        {
            return Task.FromResult(new Result(query.Value));
        }
    }
}