﻿using System;
using Brickweave.Domain;

namespace Brickweave.Messaging.ServiceBus.Tests.Models
{
    public class AnotherTestDomainEvent : IDomainEvent
    {
        public AnotherTestDomainEvent(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}