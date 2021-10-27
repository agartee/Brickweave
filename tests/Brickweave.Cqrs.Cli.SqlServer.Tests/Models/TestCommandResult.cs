namespace Brickweave.Cqrs.Cli.SqlServer.Tests.Models
{
    public class TestCommandResult
    {
        public TestCommandResult(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
