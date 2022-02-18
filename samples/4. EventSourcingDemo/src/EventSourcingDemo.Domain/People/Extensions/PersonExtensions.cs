using EventSourcingDemo.Domain.People.Models;

namespace EventSourcingDemo.Domain.People.Extensions
{
    public static class PersonExtensions
    {
        public static PersonInfo ToPersonInfo(this Person person)
        {
            return new PersonInfo(person.Id, person.Name);
        }
    }
}
