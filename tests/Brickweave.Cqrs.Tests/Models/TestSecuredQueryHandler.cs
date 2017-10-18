using System.Security.Claims;
using System.Threading.Tasks;

namespace Brickweave.Cqrs.Tests.Models
{
    public class TestSecuredQueryHandler : ISecuredQueryHandler<TestQuery, Result>
    {
        public async Task<Result> HandleAsync(TestQuery query, ClaimsPrincipal principal)
        {
            return new Result(query.Value);
        }
    }
}