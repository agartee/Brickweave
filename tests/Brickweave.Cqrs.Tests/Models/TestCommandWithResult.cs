namespace Brickweave.Cqrs.Tests.Models
{
    public class TestCommandWithResult : ICommand<Result>
    {
        public TestCommandWithResult(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}