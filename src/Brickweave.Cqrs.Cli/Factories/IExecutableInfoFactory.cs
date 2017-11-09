using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.Factories
{
    public interface IExecutableInfoFactory
    {
        ExecutableInfo Create(string[] args);
    }
}