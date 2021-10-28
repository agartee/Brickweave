namespace Brickweave.Cqrs.SqlServer.Tests.Models
{
    public class TestCommandResult
    {
        public TestCommandResult(string foo)
        {
            Foo = foo;
        }

        public string Foo { get; }
    }
}
