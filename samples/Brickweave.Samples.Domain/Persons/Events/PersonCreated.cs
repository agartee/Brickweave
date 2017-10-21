﻿using System;
using Brickweave.Messaging;

namespace Brickweave.Samples.Domain.Persons.Events
{
    public class PersonCreated : IDomainEvent
    {
        public PersonCreated(Guid id, string firstName, string lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        public Guid Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
    }
}
