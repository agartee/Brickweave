using System;

namespace Brickweave.Samples.Projection.Entities
{
    public class PersonProjection
    {
        public PersonProjection()
        {
            
        }

        public PersonProjection(Guid id, string firstName, string lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}