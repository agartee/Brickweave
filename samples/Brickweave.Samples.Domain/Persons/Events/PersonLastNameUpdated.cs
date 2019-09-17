using Brickweave.EventStore;

namespace Brickweave.Samples.Domain.Persons.Events
{
    public class PersonLastNameUpdated : IEvent
    {
        public PersonLastNameUpdated(string lastName)
        {
            LastName = lastName;
        }

        public string LastName { get; }
    }
}
