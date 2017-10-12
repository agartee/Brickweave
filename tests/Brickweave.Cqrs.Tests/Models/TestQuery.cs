namespace Brickweave.Cqrs.Tests.Models
{
    public class TestQuery : IQuery<Result>
    {
        public TestQuery(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}
