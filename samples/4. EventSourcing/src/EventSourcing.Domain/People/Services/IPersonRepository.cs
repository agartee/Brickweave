using EventSourcing.Domain.People.Models;

namespace EventSourcing.Domain.People.Services
{
    public interface IPersonRepository
    {
        Task SavePersonAsync(Person person);
        Task<Person> DemandPersonAsync(PersonId id);
        Task<IEnumerable<Person>> ListPeopleAsync();
        Task DeletePerson(PersonId id);
    }
}
