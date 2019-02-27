using System;
using Brickweave.EventStore;

namespace Brickweave.Samples.Domain.Persons.Events
{
    public class PersonCreated : IEvent
    {
        public PersonCreated(Guid personId, string firstName, string lastName)
        {
            PersonId = personId;
            FirstName = firstName;
            LastName = lastName;
        }

        public Guid PersonId { get; }
        public string FirstName { get; }
        public string LastName { get; }
    }
}
