using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Models;

namespace Brickweave.Samples.Domain.Persons.Commands
{
    public class AddSinglePersonAttribute : ICommand<PersonInfo>
    {
        /// <summary>
        /// Adds an attribute to an existing person.
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
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
