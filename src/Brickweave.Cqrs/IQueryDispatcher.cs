using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Brickweave.Cqrs
{
    public interface IQueryDispatcher
    {
        Task<object> ExecuteAsync(IQuery query, ClaimsPrincipal user = null, CancellationToken cancellationToken = default(CancellationToken));
        Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query, ClaimsPrincipal user = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}
