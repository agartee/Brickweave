using System;

namespace Brickweave.Samples.Domain.Persons.Models
{
    public class PersonInfo
    {
        public PersonInfo(Guid id, string firstName, string lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        public Guid Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
    }
}
