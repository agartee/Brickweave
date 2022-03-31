using Brickweave.Cqrs;
using EventSourcingDemo.Domain.Accounts.Models;

namespace EventSourcingDemo.Domain.Accounts.Queries
{
    public class ListAccounts : IQuery<IEnumerable<AccountInfo>>
    {
    }
}
