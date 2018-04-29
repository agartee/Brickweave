using System.Threading;
using System.Threading.Tasks;

namespace Brickweave.Cqrs.Tests.Models
{
    public class TestQueryHandler : IQueryHandler<TestQuery, Result>
    {
        public Task<Result> HandleAsync(TestQuery query, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(new Result(query.Value));
        }
    }
}