using System.Threading.Tasks;

namespace Brickweave.Cqrs.Tests.Models
{
    public class TestQueryHandler : IQueryHandler<TestQuery, Result>
    {
        public Task<Result> HandleAsync(TestQuery query)
        {
            return Task.FromResult(new Result(query.Value));
        }
    }
}