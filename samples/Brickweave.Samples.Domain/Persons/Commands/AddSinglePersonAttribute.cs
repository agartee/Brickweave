using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Models;

namespace Brickweave.Samples.Domain.Persons.Commands
{
    public class AddSinglePersonAttribute : ICommand<PersonInfo>
    {
        /// <summary>
        /// Add an attribute to an existing person. Duplicate key/values are not allowed.
        /// </summary>
        /// <param name="personId">Existing person's ID</param>
        /// <param name="key">New or existing attribute key</param>
        /// <param name="value">New attribute value</param>
        public AddSinglePersonAttribute(PersonId personId, string key, object value)
        {
            PersonId = personId;
            Key = key;
            Value = value;
        }

        public PersonId PersonId { get; }
        public string Key { get; }
        public object Value { get; }
    }
}
