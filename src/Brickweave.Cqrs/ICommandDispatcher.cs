﻿using System.Security.Claims;
using System.Threading.Tasks;

namespace Brickweave.Cqrs
{
    public interface ICommandDispatcher
    {
        Task<object> ExecuteAsync(ICommand command, ClaimsPrincipal user = null);
        Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command, ClaimsPrincipal user = null);
    }
}
