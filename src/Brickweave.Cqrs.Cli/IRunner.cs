using System.Threading.Tasks;

namespace Brickweave.Cqrs.Cli
{
    public interface IRunner
    {
        Task<object> RunAsync(string[] args);
    }
}