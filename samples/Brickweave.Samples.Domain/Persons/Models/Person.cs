using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Brickweave.EventStore;
using Brickweave.Samples.Domain.Persons.Events;
using Brickweave.Samples.Domain.Phones.Models;
using LiteGuard;

namespace Brickweave.Samples.Domain.Persons.Models
{
    public class Person : EventSourcedAggregateRoot
    {
        private readonly Dictionary<string, List<object>> _attributes =
            new Dictionary<string, List<object>>();

        private readonly List<Phone> _phones = new List<Phone>();

        Person()
        {
            Register<PersonCreated>(Apply);
            Register<PersonNameChanged>(Apply);
            Register<PersonPhoneAdded>(Apply);
            Register<PersonPhoneRemoved>(Apply);
            Register<PersonAttributeAdded>(Apply);
            Register<PersonAttributeRemoved>(Apply);
        }

        public Person(PersonId id, Name name) : this()
        {
            RaiseEvent(new PersonCreated(id, name.FirstName, name.LastName));
        }

        public Person(IEnumerable<IEvent> events) : this()
        {
            ApplyEvents(events);
        }

        public PersonId Id { get; private set; }
        public Name Name { get; private set; }

        public IEnumerable<Phone> Phones => _phones.ToArray();
        public IReadOnlyDictionary<string, IEnumerable<object>> Attributes =>
            _attributes.ToImmutableDictionary(kvp => kvp.Key, kvp => (IEnumerable<object>)kvp.Value);

        public void ChangeName(Name name)
        {
            RaiseEvent(new PersonNameChanged(name.FirstName, name.LastName));
        }

        public void AddPhone(PhoneId id, string number)
        {
            RaiseEvent(new PersonPhoneAdded(id, number));
        }

        public void RemovePhone(PhoneId id)
        {
            RaiseEvent(new PersonPhoneRemoved(id));
        }

        public void AddAttribute(string name, object value)
        {
            Guard.AgainstNullArgument(nameof(name), name);
            Guard.AgainstNullArgument(nameof(value), value);

            RaiseEvent(new PersonAttributeAdded(name, value));
        }

        public void RemoveAttribute(string name)
        {
            Guard.AgainstNullArgument(nameof(name), name);

            RaiseEvent(new PersonAttributeRemoved(name, PersonAttributeRemoved.ALL_VALUES));
        }

        public void RemoveAttribute(string attributeName, object value)
        {
            Guard.AgainstNullArgument(nameof(attributeName), attributeName);
            Guard.AgainstNullArgument(nameof(value), value);

            RaiseEvent(new PersonAttributeRemoved(attributeName, value));
        }

        private void Apply(PersonCreated @event)
        {
            Id = @event.PersonId;
            Name = new Name(@event.FirstName, @event.LastName);
        }

        private void Apply(PersonNameChanged @event)
        {
            Name = new Name(@event.FirstName, @event.LastName);
        }

        private void Apply(PersonPhoneAdded @event)
        {
            _phones.Add(new Phone(@event.PhoneId, @event.Number, EventQueue, EventRouter));
        }

        private void Apply(PersonPhoneRemoved @event)
        {
            _phones.RemoveAll(p => p.Id.Equals(@event.PhoneId));
        }

        private void Apply(PersonAttributeAdded @event)
        {
            if (_attributes.ContainsKey(@event.Name))
                _attributes[@event.Name].Add(@event.Value);
            else
                _attributes.Add(@event.Name, new List<object> { @event.Value });
        }

        private void Apply(PersonAttributeRemoved @event)
        {
            if (!_attributes.ContainsKey(@event.Name))
                return;

            if (@event.Value.Equals(PersonAttributeRemoved.ALL_VALUES))
                _attributes.Remove(@event.Name);
            else
            {
                _attributes[@event.Name].Remove(@event.Value);

                if(!_attributes[@event.Name].Any())
                    _attributes.Remove(@event.Name);
            }
        }
    }
}
