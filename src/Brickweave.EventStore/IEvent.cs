namespace Brickweave.EventStore
{
    public interface IEvent
    {
    }

    public interface IChildEvent : IEvent
    {
        object GetEntityId();
    }
}
