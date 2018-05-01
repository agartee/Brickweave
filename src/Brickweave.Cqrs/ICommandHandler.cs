using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Brickweave.Cqrs
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command, CancellationToken cancellationToken = default(CancellationToken));
    }

    public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
    {
        Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default(CancellationToken));
    }

    public interface ISecuredCommandHandler<in TCommand> : ISecured where TCommand : ICommand
    {
        Task HandleAsync(TCommand command, ClaimsPrincipal user, CancellationToken cancellationToken = default(CancellationToken));
    }

    public interface ISecuredCommandHandler<in TCommand, TResult> : ISecured where TCommand : ICommand<TResult>
    {
        Task<TResult> HandleAsync(TCommand command, ClaimsPrincipal user, CancellationToken cancellationToken = default(CancellationToken));
    }
}
