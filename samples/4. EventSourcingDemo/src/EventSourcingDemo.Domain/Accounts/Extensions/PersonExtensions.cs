using EventSourcingDemo.Domain.Accounts.Models;
using EventSourcingDemo.Domain.Common.Models;

namespace EventSourcingDemo.Domain.Accounts.Extensions
{
    public static class AccountExtensions
    {
        public static AccountInfo ToAccountInfo(this Account account, LegalEntityInfo accountHolder)
        {
            return new AccountInfo(
                account.Id, 
                account.Name, 
                accountHolder, 
                account.Balance);
        }
    }
}
