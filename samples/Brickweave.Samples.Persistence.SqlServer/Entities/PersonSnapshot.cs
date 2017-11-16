using System;
using Brickweave.Samples.Domain.Persons.Models;

namespace Brickweave.Samples.Persistence.SqlServer.Entities
{
    public class PersonSnapshot
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public PersonInfo ToInfo()
        {
            return new PersonInfo(
                new PersonId(Id),
                new Name(FirstName, LastName));
        }
    }
}