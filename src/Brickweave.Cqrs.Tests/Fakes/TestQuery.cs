namespace Brickweave.Cqrs.Tests.Fakes
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
