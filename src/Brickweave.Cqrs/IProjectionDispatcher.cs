using System.Threading.Tasks;

namespace Brickweave.Cqrs
{
    public interface IProjectionDispatcher
    {
        Task ExecuteAsync(ICommand command);
    }
}
