using Brickweave.Cqrs;
using EventSourcingDemo.Domain.People.Models;

namespace EventSourcingDemo.Domain.People.Queries
{
    public class ListPeople : IQuery<IEnumerable<PersonInfo>>
    {
    }
}
