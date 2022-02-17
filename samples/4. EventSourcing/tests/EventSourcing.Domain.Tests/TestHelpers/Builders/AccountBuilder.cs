using System.Collections.Generic;
using EventSourcing.Domain.Accounts.Models;
using EventSourcing.Domain.Common.Models;
using EventSourcing.Domain.People.Models;

namespace EventSourcing.Domain.Tests.TestHelpers.Builders
{
    public class AccountBuilder
    {
        private AccountId _id = AccountId.NewId();
        private Name _name;
        private LegalEntityId _accountHolderId;
        private readonly List<decimal> _transactionAmounts = new List<decimal>();

        public AccountBuilder WithId(AccountId id)
        {
            _id = id;
            return this;
        }

        public AccountBuilder WithName(Name name)
        {
            _name = name;
            return this;
        }

        public AccountBuilder WithAccountHolderId(LegalEntityId accountHolderId)
        {
            _accountHolderId = accountHolderId;
            return this;
        }

        public AccountBuilder WithDeposit(decimal amount)
        {
            if (amount < 0)
                throw new System.ArgumentOutOfRangeException(nameof(amount));

            _transactionAmounts.Add(amount);
            return this;
        }

        public AccountBuilder WithWithdrawal(decimal amount)
        {
            if(amount < 0)
                throw new System.ArgumentOutOfRangeException(nameof(amount));

            _transactionAmounts.Add(amount);
            return this;
        }

        public Account Build()
        {
            var account = new Account(
                _id, 
                _name ?? new Name(_id.ToString()), 
                (dynamic) _accountHolderId ?? PersonId.NewId());
            
            foreach(var amount in _transactionAmounts)
            {
                if(amount > 0)
                    account.MakeDeposit(amount);
                else
                    account.MakeWithdrawal(-amount);
            }

            account.ClearUncommittedEvents();
            account.ClearDomainEvents();

            return account;
        }
    }
}
