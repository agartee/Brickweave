using EventSourcingDemo.Domain.Common.Models;
using EventSourcingDemo.Domain.People.Models;
using EventSourcingDemo.SqlServer.Entities;

namespace EventSourcingDemo.SqlServer.Extensions
{
    public static class PersonExtensions
    {
        public static Person ToPerson(this PersonData data)
        {
            return new Person(
                new Domain.People.Models.PersonId(data.Id),
                new Name(data.Name));
        }

        public static PersonInfo ToPersonInfo(this PersonData data)
        {
            return new PersonInfo(
                new Domain.People.Models.PersonId(data.Id),
                new Name(data.Name));
        }
    }
}
