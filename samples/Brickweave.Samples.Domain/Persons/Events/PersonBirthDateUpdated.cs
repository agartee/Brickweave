using System;
using Brickweave.EventStore;

namespace Brickweave.Samples.Domain.Persons.Events
{
    public class PersonBirthDateUpdated : IEvent
    {
        public PersonBirthDateUpdated(DateTime birthDate)
        {
            BirthDate = birthDate;
        }

        public DateTime BirthDate { get; }
    }
}
