using Brickweave.Cqrs;
using EventSourcing.Domain.Accounts.Models;

namespace EventSourcing.Domain.Accounts.Queries
{
    public class ListAccounts : IQuery<IEnumerable<AccountInfo>>
    {
        
    }
}
