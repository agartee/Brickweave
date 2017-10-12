namespace Brickweave.Domain.Tests.Models
{
    public class TestDomainModel : AggregateRoot
    {
        public void RaiseDomainEvent(string bar)
        {
            EnqueueDomainEvent(new TestDomainEvent(bar));
        }

        public void NullFoo()
        {
            EnqueueDomainEvent(null);
        }
    }
}