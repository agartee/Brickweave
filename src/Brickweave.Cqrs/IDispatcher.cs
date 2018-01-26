using System.Security.Claims;
using System.Threading.Tasks;

namespace Brickweave.Cqrs
{
    public interface IDispatcher
    {
        Task<object> DispatchCommandAsync(ICommand command, ClaimsPrincipal user = null);
        Task<TResult> DispatchCommandAsync<TResult>(ICommand<TResult> command, ClaimsPrincipal user = null);

        Task<object> DispatchQueryAsync(IQuery query, ClaimsPrincipal user = null);
        Task<TResult> DispatchQueryAsync<TResult>(IQuery<TResult> query, ClaimsPrincipal user = null);
    }
}
