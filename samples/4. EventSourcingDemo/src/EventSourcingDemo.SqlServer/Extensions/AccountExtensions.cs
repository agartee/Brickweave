using EventSourcingDemo.Domain.Accounts.Models;
using EventSourcingDemo.Domain.Common.Models;
using EventSourcingDemo.Domain.Companies.Models;
using EventSourcingDemo.Domain.People.Models;
using EventSourcingDemo.SqlServer.Entities;

namespace EventSourcingDemo.SqlServer.Extensions
{
    public static class AccountExtensions
    {
        public static bool IsPersonalAccount(this Account account)
        {
            return account.AccountHolderId is Domain.People.Models.PersonId;
        }

        public static bool IsBusinessAccount(this Account account)
        {
            return account.AccountHolderId is CompanyId;
        }

        public static AccountInfo ToAccountInfo(this PersonalAccountData data)
        {
            return new AccountInfo(
                new AccountId(data.Id),
                new Name(data.Name),
                new PersonInfo(
                    new PersonId(data.AccountHolder.Id),
                    new Name(data.AccountHolder.Name)),
                data.Balance);
        }

        public static AccountInfo ToAccountInfo(this BusinessAccountData data)
        {
            return new AccountInfo(
                new AccountId(data.Id),
                new Name(data.Name), 
                new CompanyInfo(
                    new CompanyId(data.AccountHolderId),
                    new Name(data.AcountHolder.Name)),
                data.Balance);
        }
    }
}
