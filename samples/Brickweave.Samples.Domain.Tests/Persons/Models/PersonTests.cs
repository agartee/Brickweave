using System.Linq;
using Brickweave.Samples.Domain.Persons.Events;
using Brickweave.Samples.Domain.Persons.Models;
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

            @event.Id.Should().Be(id.Value);
            @event.FirstName.Should().Be(firstName);
            @event.LastName.Should().Be(lastName);
        }

        [Fact]
        public void AddPhone_AddsPhoneToCollectionAndAddsEvents()
        {
            var person = new Person(PersonId.NewId(), new Name("Adam", "Gartee"));
            person.ClearUncommittedEvents();

            var id = PhoneId.NewId();
            var number = "(555) 555-1111";

            person.AddPhone(id, number);

            person.GetUncommittedEvents().Should().HaveCount(1);

            var @event = person.GetUncommittedEvents()
                .First().As<PhoneAdded>();

            @event.Id.Should().Be(id.Value);
            @event.Number.Should().Be(number);
        }

        [Fact]
        public void UpdatePhone_AddsPhoneToCollectionAndAddsEvents()
        {
            var person = new Person(PersonId.NewId(), new Name("Adam", "Gartee"));
            person.AddPhone(PhoneId.NewId(), "(555) 555-1111");
            person.ClearUncommittedEvents();

            var newNumber = "(555) 555-2222";
            var phone = person.Phones.First();
            phone.UpdateNumber(newNumber);

            person.GetUncommittedEvents().Should().HaveCount(1);

            var @event = person.GetUncommittedEvents()
                .First().As<PhoneUpdated>();

            @event.Id.Should().Be(phone.Id.Value);
            @event.Number.Should().Be(newNumber);
        }
    }
}
