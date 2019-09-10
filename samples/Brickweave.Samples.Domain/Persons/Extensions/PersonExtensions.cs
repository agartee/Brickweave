using System.Linq;
using Brickweave.Samples.Domain.Persons.Models;
using Brickweave.Samples.Domain.Phones.Models;

namespace Brickweave.Samples.Domain.Persons.Extensions
{
    public static class PersonExtensions
    {
        public static PersonInfo ToInfo(this Person person)
        {
            return new PersonInfo(
                person.Id,
                person.Name,
                person.BirthDate,
                person.Phones.Select(p => new PhoneInfo(p.Id, p.PhoneType, p.Number)).ToArray(),
                person.Attributes);
        }
    }
}
