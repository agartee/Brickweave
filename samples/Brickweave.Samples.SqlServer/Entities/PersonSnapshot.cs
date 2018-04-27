using System;
using System.ComponentModel.DataAnnotations.Schema;
using Brickweave.Samples.Domain.Persons.Models;

namespace Brickweave.Samples.SqlServer.Entities
{
    [Table(TABLE_NAME)]
    public class PersonSnapshot
    {
        public const string TABLE_NAME = "Person";

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