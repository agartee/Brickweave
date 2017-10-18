using System.Security.Claims;
using System.Threading.Tasks;

namespace Brickweave.Cqrs
{
    public interface IQueryExecutor
    {
        Task<object> ExecuteAsync(IQuery query, ClaimsPrincipal user = null);
        Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query, ClaimsPrincipal user = null);
    }
}
