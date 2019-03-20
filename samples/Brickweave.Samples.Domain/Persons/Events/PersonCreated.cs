using System;
using Brickweave.EventStore;
using Brickweave.Samples.Domain.Persons.Models;

namespace Brickweave.Samples.Domain.Persons.Events
{
    public class PersonCreated : IEvent
    {
        public PersonCreated(PersonId personId, string firstName, string lastName)
        {
            PersonId = personId;
            FirstName = firstName;
            LastName = lastName;
        }

        public PersonId PersonId { get; }
        public string FirstName { get; }
        public string LastName { get; }
    }
}
