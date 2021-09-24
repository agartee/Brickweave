namespace BasicCqrs.Domain.People.Models
{
    public class Person
    {
        public Person(PersonId id)
        {
            Id = id;
        }

        public PersonId Id { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
