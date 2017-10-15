using Brickweave.Cqrs;

namespace Brickweave.Cqrs.Cli.Tests.Models
{
    public class CreateBar : ICommand
    {
        public string Id { get; set; }
    }
}
