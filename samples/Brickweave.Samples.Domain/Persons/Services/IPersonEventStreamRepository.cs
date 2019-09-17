using System.Collections.Generic;
using System.Threading.Tasks;
using Brickweave.EventStore;
using Brickweave.Samples.Domain.Persons.Models;

namespace Brickweave.Samples.Domain.Persons.Services
{
    public interface IPersonEventStreamRepository
    {
        Task<LinkedList<IEvent>> GetPersonEventStreamJsonAsync(PersonId id);
    }
}
