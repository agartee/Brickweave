using System.Threading.Tasks;

namespace Brickweave.Cqrs
{
    public interface ICommandProcessor
    {
        Task<object> ProcessAsync(ICommand command);
        Task<TResult> ProcessAsync<TResult>(ICommand<TResult> command);
    }
}
