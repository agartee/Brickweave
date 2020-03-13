using System;
using Brickweave.Domain;
using Brickweave.EventStore;
using Brickweave.Samples.Domain.Persons.Models;

namespace Brickweave.Samples.Domain.Persons.Events
{
    public class PersonCreated : IEvent, IDomainEvent
    {
        public PersonCreated(PersonId personId, string firstName, string lastName, DateTime? birthDate = null)
        {
            PersonId = personId;
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
        }

        public PersonId PersonId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public DateTime? BirthDate { get; }
    }
}
