using Brickweave.EventStore;

namespace Brickweave.Samples.Domain.Persons.Events
{
    public class PersonFirstNameUpdated : IEvent
    {
        public PersonFirstNameUpdated(string firstName)
        {
            FirstName = firstName;
        }

        public string FirstName { get; }
    }
}
