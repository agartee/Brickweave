namespace Brickweave.Cqrs.SqlServer.Tests.Models
{
    public class TestCommand : ICommand<TestCommandResult>
    {
        public TestCommand(string foo)
        {
            Foo = foo;
        }
        
        public string Foo { get; }
    }
}
