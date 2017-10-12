using System.Threading.Tasks;

namespace Brickweave.Cqrs.Tests.Models
{
    public class TestQueryHandler : IQueryHandler<TestQuery, Result>
    {
        public async Task<Result> HandleAsync(TestQuery query)
        {
            return new Result(query.Value);
        }
    }
}