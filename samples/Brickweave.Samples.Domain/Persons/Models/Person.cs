using System.Collections.Generic;
using System.Linq;
using Brickweave.EventStore;
using Brickweave.Samples.Domain.Persons.Events;

namespace Brickweave.Samples.Domain.Persons.Models
{
    public class Person : EventSourcedAggregateRoot
    {
        private readonly List<Phone> _phones = new List<Phone>();

        Person()
        {
            Register<PersonCreated>(Apply);
            Register<NameChanged>(Apply);
            Register<PhoneAdded>(Apply);
        }

        public Person(PersonId id, Name name) : this()
        {
            RaiseEvent(new PersonCreated(id.Value, name.FirstName, name.LastName));
        }

        public Person(IEnumerable<IEvent> events) : this()
        {
            ApplyEvents(events);
        }

        public PersonId Id { get; private set; }
        public Name Name { get; private set; }

        public IEnumerable<Phone> Phones => _phones.ToArray();

        public void ChangeName(Name name)
        {
            RaiseEvent(new NameChanged(name.FirstName, name.LastName));
        }

        public void AddPhone(PhoneId id, string number)
        {
            RaiseEvent(new PhoneAdded(id.Value, number));
        }

        public PersonInfo ToInfo()
        {
            return new PersonInfo(
                Id, 
                Name, 
                Phones.Select(p => new PhoneInfo(p.Id, p.Number)).ToArray());
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

        private void Apply(PhoneAdded @event)
        {
            _phones.Add(new Phone(new PhoneId(@event.Id), @event.Number, EventQueue, Router));
        }
    }
}
