using Brickweave.EventStore;

namespace Brickweave.Samples.Domain.Persons.Events
{
    public class PersonNameChanged : IEvent
    {
        public PersonNameChanged(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public string FirstName { get; }
        public string LastName { get; }
    }
}
