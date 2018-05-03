namespace Brickweave.EventStore.Tests.Models
{
    public class ChildAdded : IEvent
    {
        public ChildAdded(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
