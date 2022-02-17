using EventSourcing.Domain.Accounts.Models;

namespace EventSourcing.Domain.Accounts.Services
{
    public interface IAccountRepository
    {
        Task SaveAccountAsync(Account account);
        Task<Account> DemandAccountAsync(AccountId id);
        Task<IEnumerable<Account>> ListAccountsAsync();
        Task DeleteAccount(AccountId id);
    }
}
