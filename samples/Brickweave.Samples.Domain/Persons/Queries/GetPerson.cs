using System;
using Brickweave.Cqrs;
using Brickweave.Samples.Domain.Persons.Models;

namespace Brickweave.Samples.Domain.Persons.Queries
{
    public class GetPerson : IQuery<PersonInfo>
    {
        /// <summary>
        /// Get an existing person.
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="pointInTime"></param>
        public GetPerson(PersonId personId, DateTime? pointInTime = null)
        {
            PersonId = personId;
            PointInTime = pointInTime;
        }
         
        public PersonId PersonId { get; }
        public DateTime? PointInTime { get; }
    }
}
