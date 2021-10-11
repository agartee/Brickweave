using System;

namespace Brickweave.Cqrs.Cli.Tests.Models
{
    public class CreateQux : ICommand<string>
    {
        public int Id { get; init; }
        public string Bar { get; init; } = "bar";
        public DateTime DateCreated { get; init; }
    }
}
