﻿using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Brickweave.Cqrs
{
    public interface IDispatcher
    {
        Task<object> DispatchCommandAsync(ICommand command, ClaimsPrincipal user = null, CancellationToken cancellationToken = default(CancellationToken));
        Task<TResult> DispatchCommandAsync<TResult>(ICommand<TResult> command, ClaimsPrincipal user = null, CancellationToken cancellationToken = default(CancellationToken));

        Task<object> DispatchQueryAsync(IQuery query, ClaimsPrincipal user = null, CancellationToken cancellationToken = default(CancellationToken));
        Task<TResult> DispatchQueryAsync<TResult>(IQuery<TResult> query, ClaimsPrincipal user = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}
