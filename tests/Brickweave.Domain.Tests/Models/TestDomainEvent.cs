namespace Brickweave.Domain.Tests.Models
{
    public class TestDomainEvent : IDomainEvent
    {
        public TestDomainEvent(string bar)
        {
            Bar = bar;
        }

        public string Bar { get; }
    }
}