using System.Text;

namespace Brickweave.Messaging.ServiceBus
{
    public class Utf8Encoder : IMessageEncoder
    {
        public string Name => "UTF8";

        public byte[] Encode(string content)
        {
            return Encoding.UTF8.GetBytes(content);
        }
    }
}