using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.Factories
{
    public interface IHelpInfoFactory
    {
        HelpInfo Create(string[] args);
    }
}