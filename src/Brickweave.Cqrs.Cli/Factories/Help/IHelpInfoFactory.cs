using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.Factories.Help
{
    public interface IHelpInfoFactory
    {
        HelpInfo Create(string[] args);
    }
}