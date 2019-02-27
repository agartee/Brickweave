using System.Collections.Generic;
using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Models;

namespace Brickweave.Samples.Domain.Persons.Commands
{
    public class AddMultiplePersonAttributes : ICommand<PersonInfo>
    {
        /// <summary>
        /// Adds attributes to an existing person. Supports multi-value attributes (same key, multiple values).
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="attributes"></param>
        public AddMultiplePersonAttributes(PersonId personId, Dictionary<string, List<object>> attributes)
        {
            PersonId = personId;
            Attributes = attributes;
        }

        public PersonId PersonId { get; }
        public Dictionary<string, List<object>> Attributes { get; } = new Dictionary<string, List<object>>();
    }
}
