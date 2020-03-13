using System;
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
            Register<PersonFirstNameUpdated>(Apply);
            Register<PersonLastNameUpdated>(Apply);
            Register<PersonBirthDateUpdated>(Apply);
            Register<PersonPhoneAdded>(Apply);
            Register<PersonPhoneRemoved>(Apply);
            Register<PersonAttributeSet>(Apply);
            Register<PersonAttributeRemoved>(Apply);
            Register<PersonDeleted>(Apply);
        }

        public Person(PersonId id, Name name, DateTime? birthDate = null) : this()
        {
            RaiseEvent(new PersonCreated(id, name.FirstName, name.LastName, birthDate));
        }

        public Person(IEnumerable<IEvent> events) : this()
        {
            ApplyEvents(events);
        }

        public PersonId Id { get; private set; }
        public Name Name { get; private set; }
        public DateTime? BirthDate { get; private set; }

        public IEnumerable<Phone> Phones => _phones.ToArray();
        public IReadOnlyDictionary<string, IEnumerable<object>> Attributes =>
            _attributes.ToImmutableDictionary(kvp => kvp.Key, kvp => (IEnumerable<object>)kvp.Value);

        public bool IsActive { get; private set; }

        public static Person CreateFromEvents(IEnumerable<IEvent> events)
        {
            var person = new Person();

            events.ToList().ForEach(e => person.RaiseEvent(e));

            return person;
        }

        public void SetFirstName(string firstName)
        {
            RaiseEvent(new PersonFirstNameUpdated(firstName));
        }

        public void SetLastName(string lastName)
        {
            RaiseEvent(new PersonLastNameUpdated(lastName));
        }

        public void SetBirthDate(DateTime birthDate)
        {
            RaiseEvent(new PersonBirthDateUpdated(birthDate));
        }

        public void ChangeName(Name name)
        {
            RaiseEvent(new PersonNameChanged(name.FirstName, name.LastName));
        }

        public void AddPhone(PhoneId id, PhoneType phoneType, string number)
        {
            RaiseEvent(new PersonPhoneAdded(id, phoneType, number));
        }

        public void RemovePhone(PhoneId id)
        {
            RaiseEvent(new PersonPhoneRemoved(id));
        }

        public void AddAttribute(string name, object value)
        {
            Guard.AgainstNullArgument(nameof(name), name);
            Guard.AgainstNullArgument(nameof(value), value);

            if (_attributes.ContainsKey(name) && _attributes[name].Contains(value))
                throw new InvalidOperationException("duplicate attributes are not allowed.");

            RaiseEvent(new PersonAttributeSet(name, value));
        }

        public void RemoveAttribute(string name)
        {
            Guard.AgainstNullArgument(nameof(name), name);

            if (!_attributes.ContainsKey(name))
                return;

            RaiseEvent(new PersonAttributeRemoved(name, PersonAttributeRemoved.ALL_VALUES));
        }

        public void RemoveAttribute(string attributeName, object value)
        {
            Guard.AgainstNullArgument(nameof(attributeName), attributeName);
            Guard.AgainstNullArgument(nameof(value), value);

            RaiseEvent(new PersonAttributeRemoved(attributeName, value));
        }

        public void Delete()
        {
            RaiseEvent(new PersonDeleted());
        }

        private void Apply(PersonCreated @event)
        {
            Id = @event.PersonId;
            Name = new Name(@event.FirstName, @event.LastName);
            BirthDate = @event.BirthDate;
            IsActive = true;
        }

        private void Apply(PersonNameChanged @event)
        {
            Name = new Name(@event.FirstName, @event.LastName);
        }

        private void Apply(PersonFirstNameUpdated @event)
        {
            Name = new Name(@event.FirstName, Name.LastName);
        }

        private void Apply(PersonLastNameUpdated @event)
        {
            Name = new Name(Name.FirstName, @event.LastName);
        }

        private void Apply(PersonBirthDateUpdated @event)
        {
            BirthDate = @event.BirthDate;
        }

        private void Apply(PersonPhoneAdded @event)
        {
            _phones.Add(new Phone(@event.PhoneId, @event.PhoneType, @event.Number, 
                EventQueue, DomainEventQueue, EventRouter));
        }

        private void Apply(PersonPhoneRemoved @event)
        {
            _phones.RemoveAll(p => p.Id.Equals(@event.PhoneId));
        }

        private void Apply(PersonAttributeSet @event)
        {
            if (_attributes.ContainsKey(@event.Name))
                _attributes[@event.Name].Add(@event.Value);
            else
                _attributes.Add(@event.Name, new List<object> { @event.Value });
        }

        private void Apply(PersonAttributeRemoved @event)
        {
            // Note that we should not check of the attribute exists in this handler
            // method. That check belongs in the public method before determining 
            // whether to raise the event in the first place.

            if (@event.Value.Equals(PersonAttributeRemoved.ALL_VALUES))
                _attributes.Remove(@event.Name);
            else
            {
                _attributes[@event.Name].Remove(@event.Value);

                if(!_attributes[@event.Name].Any())
                    _attributes.Remove(@event.Name);
            }
        }

        private void Apply(PersonDeleted @event)
        {
            IsActive = false;
        }
    }
}
