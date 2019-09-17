using System.Collections.Generic;
using System.Linq;
using Brickweave.EventStore;
using Brickweave.Samples.Domain.Phones.Events;
using Brickweave.Samples.Domain.Phones.Models;
using FluentAssertions;
using Xunit;

namespace Brickweave.Samples.Domain.Tests.Phones.Models
{
    public class PhoneTests
    {
        [Fact]
        public void UpdatePhone_UpdatesPhoneInCollectionAndAddsEvents()
        {
            var id = PhoneId.NewId();
            var type = PhoneType.Home;
            var number = "(555) 555-1111";
            var eventQueue = new Queue<IEvent>();
            var eventRouter = new RegistrationEventRouter();

            var phone = new Phone(id, type, number, eventQueue, eventRouter);

            var newNumber = "(555) 555-2222";
            phone.UpdateNumber(newNumber);

            eventQueue.Should().HaveCount(1);

            var @event = eventQueue
                .First().As<PhoneNumberUpdated>();

            @event.PhoneId.Should().Be(phone.Id.Value);
            @event.Number.Should().Be(newNumber);
        }
    }
}
