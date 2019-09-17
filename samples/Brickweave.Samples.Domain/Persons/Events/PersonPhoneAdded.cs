using Brickweave.EventStore;
using Brickweave.Samples.Domain.Phones.Models;

namespace Brickweave.Samples.Domain.Persons.Events
{
    public class PersonPhoneAdded : IEvent
    {
        public PersonPhoneAdded(PhoneId phoneId, PhoneType phoneType, string number)
        {
            PhoneId = phoneId;
            PhoneType = phoneType;
            Number = number;
        }

        public PhoneId PhoneId { get; }
        public PhoneType PhoneType { get; }
        public string Number { get; }
    }
}
