using System;
using System.Threading.Tasks;
using Brickweave.Samples.Domain.Persons.Models;

namespace Brickweave.Samples.Domain.Persons.Services
{
    public interface IPersonRepository
    {
        Task SaveAsync(Person person);
        Task<Person> GetPersonAsync(Guid id);
    }
}
