using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.Readers
{
    public interface ICategoryHelpReader
    {
        HelpInfo GetHelpInfo(HelpAdjacencyCriteria adjacencyCriteria);
    }
}
