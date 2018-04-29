using System.Collections.Generic;
using Brickweave.EventStore;
using Brickweave.Samples.Domain.Persons.Events;

namespace Brickweave.Samples.Domain.Persons.Models
{
    public class Person : EventSourcedAggregateRoot
    {
        Person()
        {
            Register<PersonCreated>(Apply);
            Register<NameChanged>(Apply);
        }

        public Person(PersonId id, Name name) : this()
        {
            RaiseEvent(new PersonCreated(id.Value, name.FirstName, name.LastName));
        }

        public Person(IEnumerable<IAggregateEvent> events) : this()
        {
            ApplyEvents(events);
        }

        public PersonId Id { get; private set; }

        public Name Name { get; private set; }

        public void ChangeName(Name name)
        {
            RaiseEvent(new NameChanged(name.FirstName, name.LastName));
        }

        public PersonInfo ToInfo()
        {
            return new PersonInfo(Id, Name);
        }

        private void Apply(PersonCreated @event)
        {
            Id = new PersonId(@event.Id);
            Name = new Name(@event.FirstName, @event.LastName);
        }

        private void Apply(NameChanged @event)
        {
            Name = new Name(@event.FirstName, @event.LastName);
        }
    }
}