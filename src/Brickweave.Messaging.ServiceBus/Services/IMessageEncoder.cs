namespace Brickweave.Messaging.ServiceBus.Services
{
    public interface IMessageEncoder
    {
        string Name { get; }
        byte[] Encode(string content);
    }
}
