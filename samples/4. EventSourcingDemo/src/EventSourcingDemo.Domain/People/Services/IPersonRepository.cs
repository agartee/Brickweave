using EventSourcingDemo.Domain.People.Models;

namespace EventSourcingDemo.Domain.People.Services
{
    public interface IPersonRepository
    {
        Task SavePersonAsync(Person person);
        Task<Person> DemandPersonAsync(PersonId id);
        Task<IEnumerable<PersonInfo>> ListPeopleAsync();
        Task DeletePersonAsync(PersonId id);
    }
}
