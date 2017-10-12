using System;

namespace Brickweave.Samples.Domain.Persons.Models
{
    public class Person
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public PersonInfo ToInfo()
        {
            return new PersonInfo(Id, FirstName, LastName);
        }
    }
}
