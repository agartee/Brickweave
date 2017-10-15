using System.Threading.Tasks;

namespace Brickweave.Cqrs
{
    public interface ICommandExecutor
    {
        Task<object> ExecuteAsync(ICommand command);
        Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command);
    }
}
