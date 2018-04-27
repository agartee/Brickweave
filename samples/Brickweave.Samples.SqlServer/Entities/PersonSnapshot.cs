using Brickweave.Samples.Domain.Persons.Models;
using System;

namespace Brickweave.Samples.SqlServer.Entities
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