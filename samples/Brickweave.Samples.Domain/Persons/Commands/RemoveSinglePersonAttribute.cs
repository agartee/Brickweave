using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Models;

namespace Brickweave.Samples.Domain.Persons.Commands
{
    public class RemoveSinglePersonAttribute : ICommand<PersonInfo>
    {
        /// <summary>
        /// Remove an attribute from an existing person.
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="key"></param>
        public RemoveSinglePersonAttribute(PersonId personId, string key)
        {
            PersonId = personId;
            Key = key;
        }

        public PersonId PersonId { get; }
        public string Key { get; }
    }
}
