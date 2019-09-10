using System.Linq;
using Brickweave.Samples.Domain.Persons.Events;
using Brickweave.Samples.Domain.Persons.Models;
using Brickweave.Samples.Domain.Phones.Events;
using Brickweave.Samples.Domain.Phones.Models;
using FluentAssertions;
using Xunit;

namespace Brickweave.Samples.Domain.Tests.Persons.Models
{
    public class PersonTests
    {
        [Fact]
        public void Created_SetsPropertiesAndAddsEvent()
        {
            var id = PersonId.NewId();
            var firstName = "Adam";
            var lastName = "Gartee";

            var person = new Person(id, new Name(firstName, lastName));

            person.Id.Should().Be(id);
            person.Name.FirstName.Should().Be(firstName);
            person.Name.LastName.Should().Be(lastName);

            person.GetUncommittedEvents().Should().HaveCount(1);

            var @event = person.GetUncommittedEvents()
                .First().As<PersonCreated>();

            @event.PersonId.Should().Be(id.Value);
            @event.FirstName.Should().Be(firstName);
            @event.LastName.Should().Be(lastName);
        }

        [Fact]
        public void AddPhone_AddsPhoneToCollectionAndAddsEvents()
        {
            var person = new Person(PersonId.NewId(), new Name("Adam", "Gartee"));
            person.ClearUncommittedEvents();

            var id = PhoneId.NewId();
            var type = PhoneType.Home;
            var number = "(555) 555-1111";

            person.AddPhone(id, type, number);

            person.GetUncommittedEvents().Should().HaveCount(1);

            var @event = person.GetUncommittedEvents()
                .First().As<PersonPhoneAdded>();

            @event.PhoneId.Should().Be(id.Value);
            @event.Number.Should().Be(number);
        }

        [Fact]
        public void RemovePhone_RemovesPhoneFromCollectionAndAddsEvents()
        {
            var person = new Person(PersonId.NewId(), new Name("Adam", "Gartee"));
            person.AddPhone(PhoneId.NewId(), PhoneType.Home, "(555) 555-1111");
            person.ClearUncommittedEvents();

            var phone = person.Phones.First();
            person.RemovePhone(phone.Id);

            person.Phones.Should().BeEmpty();

            person.GetUncommittedEvents().Should().HaveCount(1);

            var @event = person.GetUncommittedEvents()
                .First().As<PersonPhoneRemoved>();

            @event.PhoneId.Should().Be(phone.Id.Value);
        }

        [Fact]
        public void AddAttribute_AddsAttributeAndEvent()
        {
            var key = "A";
            var value = 1;

            var person = new Person(PersonId.NewId(), new Name("Adam", "Gartee"));
            person.ClearUncommittedEvents();

            person.AddAttribute(key, value);

            person.Attributes[key].Should().Contain(value);

            person.GetUncommittedEvents().Should().HaveCount(1);

            var @event = person.GetUncommittedEvents().First()
                .As<PersonAttributeSet>();

            @event.Name.Should().Be(key);
            @event.Value.Should().Be(value);
        }

        [Fact]
        public void RemoveAttribute_ByKey_RemovesAttributeAndAddsEvent()
        {
            var key = "A";

            var person = new Person(PersonId.NewId(), new Name("Adam", "Gartee"));
            person.AddAttribute(key, 1);
            person.ClearUncommittedEvents();

            person.RemoveAttribute(key);

            person.Attributes.Should().BeEmpty();

            person.GetUncommittedEvents().Should().HaveCount(1);

            var @event = person.GetUncommittedEvents().First()
                .As<PersonAttributeRemoved>();

            @event.Name.Should().Be(key);
            @event.Value.Should().Be("*");
        }

        [Fact]
        public void RemoveAttribute_ByKeyAndValue_RemovesAttributeAndAddsEvent()
        {
            var key = "A";
            var value = 1;

            var person = new Person(PersonId.NewId(), new Name("Adam", "Gartee"));
            person.AddAttribute(key, value);
            person.ClearUncommittedEvents();

            person.RemoveAttribute(key, value);

            person.Attributes.Should().BeEmpty();

            person.GetUncommittedEvents().Should().HaveCount(1);

            var @event = person.GetUncommittedEvents().First()
                .As<PersonAttributeRemoved>();

            @event.Name.Should().Be(key);
            @event.Value.Should().Be(value);
        }
    }
}
