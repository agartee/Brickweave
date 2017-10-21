namespace Brickweave.Messaging.Serialization
{
    public interface IMessageSerializer
    {
        T DeserializeObject<T>(string json);
        string SerializeObject(object obj);
    }
}