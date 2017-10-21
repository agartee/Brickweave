namespace Brickweave.Messaging.ServiceBus
{
    public interface IMessageEncoder
    {
        string Name { get; }
        byte[] Encode(string content);
    }
}
