using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.Parsers
{
    public interface IArgParser
    {
        ExecutableInfo Parse(string[] args);
    }
}