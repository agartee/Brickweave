using System.Threading.Tasks;
using Brickweave.Samples.Domain.Persons.Models;

namespace Brickweave.Samples.Domain.Persons.Services
{
    public interface IPersonRepository
    {
        Task SavePersonAsync(Person person);
        Task<Person> GetPersonAsync(PersonId id);
    }
}
