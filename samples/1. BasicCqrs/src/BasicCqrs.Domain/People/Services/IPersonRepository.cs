using System.Collections.Generic;
using System.Threading.Tasks;
using BasicCqrs.Domain.People.Models;

namespace BasicCqrs.Domain.People.Services
{
    public interface IPersonRepository
    {
        Task SavePersonAsync(Person person);
        Task<Person> GetPersonAsync(PersonId id);
        Task<IEnumerable<Person>> ListPeopleAsync(PersonSearchCriteria criteria);
        Task DeletePerson(PersonId id);
    }
}
