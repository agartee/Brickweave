using System.Collections.Generic;
using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Models;

namespace Brickweave.Samples.Domain.Persons.Commands
{
    public class AddMultiplePersonAttributes : ICommand<PersonInfo>
    {
        /// <summary>
        /// Add attributes to an existing person. Supports multi-value attributes (same key, multiple values). Duplicate key/values are not allowed.
        /// </summary>
        /// <param name="personId">Existing person's ID</param>
        /// <param name="attributes">Key/value pairs representing attributes.</param>
        public AddMultiplePersonAttributes(PersonId personId, Dictionary<string, List<object>> attributes)
        {
            PersonId = personId;
            Attributes = attributes;
        }

        public PersonId PersonId { get; }
        public Dictionary<string, List<object>> Attributes { get; } = new Dictionary<string, List<object>>();
    }
}
