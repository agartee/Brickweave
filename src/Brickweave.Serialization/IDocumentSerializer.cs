namespace Brickweave.Serialization
{
    public interface IDocumentSerializer
    {
        T DeserializeObject<T>(string json);
        string SerializeObject(object obj);
    }
}
