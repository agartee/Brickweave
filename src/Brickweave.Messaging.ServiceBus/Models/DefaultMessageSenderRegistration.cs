namespace Brickweave.Messaging.ServiceBus.Models
{
    public class DefaultMessageSenderRegistration
    {
        public DefaultMessageSenderRegistration(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
