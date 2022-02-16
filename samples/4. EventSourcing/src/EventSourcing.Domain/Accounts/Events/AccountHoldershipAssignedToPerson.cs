using Brickweave.EventStore;
using EventSourcing.Domain.People.Models;

namespace EventSourcing.Domain.Accounts.Events
{
    public class AccountHoldershipAssignedToPerson : IEvent
    {
        public AccountHoldershipAssignedToPerson(PersonId personId)
        {
            PersonId = personId;
        }

        public PersonId PersonId { get; }
    }
}
