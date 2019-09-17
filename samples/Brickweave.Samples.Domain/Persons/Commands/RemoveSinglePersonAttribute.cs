using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Models;

namespace Brickweave.Samples.Domain.Persons.Commands
{
    public class RemoveSinglePersonAttribute : ICommand<PersonInfo>
    {
        /// <summary>
        /// Remove an attribute from an existing person.
        /// </summary>
        /// <param name="personId">Existing person's ID</param>
        /// <param name="key">Existing attribute key</param>
        public RemoveSinglePersonAttribute(PersonId personId, string key)
        {
            PersonId = personId;
            Key = key;
        }

        public PersonId PersonId { get; }
        public string Key { get; }
    }
}
