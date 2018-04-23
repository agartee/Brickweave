using System.Threading.Tasks;

namespace Brickweave.Cqrs
{
    public interface IProjectionHandler<in TCommand>
    {
        Task HandleAsync(TCommand command);
    }
}