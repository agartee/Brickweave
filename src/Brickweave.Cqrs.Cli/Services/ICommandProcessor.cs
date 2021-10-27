using System.Threading;
using System.Threading.Tasks;

namespace Brickweave.Cqrs.Cli.Services
{
    public interface ICommandProcessor
    {
        Task ProcessCommandsAsync(CancellationToken stoppingToken);
    }
}