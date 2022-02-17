using EventSourcingDemo.Domain.Accounts.Models;

namespace EventSourcingDemo.Domain.Accounts.Services
{
    public interface IAccountRepository
    {
        Task SaveAccountAsync(Account account);
        Task<Account> DemandAccountAsync(AccountId id);
        Task<IEnumerable<AccountInfo>> ListAccountsAsync();
    }
}
