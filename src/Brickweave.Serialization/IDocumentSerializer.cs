namespace Brickweave.Serialization
{
    public interface IDocumentSerializer
    {
        T DeserializeObject<T>(string json);
        T DeserializeObject<T>(string typeName, string json);
        object DeserializeObject(string typeName, string json);
        string SerializeObject(object obj);
    }
}
