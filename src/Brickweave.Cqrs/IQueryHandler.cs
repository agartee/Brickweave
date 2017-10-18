using System.Security.Claims;
using System.Threading.Tasks;

namespace Brickweave.Cqrs
{
    public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }

    public interface ISecuredQueryHandler<in TQuery, TResult> : ISecured where TQuery : IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query, ClaimsPrincipal user);
    }
}