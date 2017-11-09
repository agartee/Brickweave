using System.Collections.Generic;
using Brickweave.Cqrs.Cli.Models;

namespace Brickweave.Cqrs.Cli.Readers
{
    public interface IExecutableHelpReader
    {
        IEnumerable<HelpInfo> GetHelpInfo(HelpAdjacencyCriteria adjacencyCriteria);
    }
}