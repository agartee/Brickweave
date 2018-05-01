using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Brickweave.Cqrs
{
    public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default(CancellationToken));
    }

    public interface ISecuredQueryHandler<in TQuery, TResult> : ISecured where TQuery : IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query, ClaimsPrincipal user, CancellationToken cancellationToken = default(CancellationToken));
    }
}