using System.Linq;
using EventSourcing.Domain.Accounts.Events;
using EventSourcing.Domain.Accounts.Models;
using EventSourcing.Domain.Common.Models;
using EventSourcing.Domain.Companies.Models;
using EventSourcing.Domain.People.Models;
using EventSourcing.Domain.Tests.TestHelpers.Builders;
using FluentAssertions;
using Xunit;

namespace EventSourcing.Domain.Tests.Accounts.Models
{
    public class AccountTests
    {
        [Fact]
        public void Created_WithCompanyId_CreatesModelAndRaisesEvents()
        {
            var id = AccountId.NewId();
            var name = new Name("account1");
            var companyId = CompanyId.NewId();

            var result = new Account(id, name, companyId);

            result.Id.Should().Be(id);
            result.Name.Should().Be(name);
            result.AccountHolderId.Should().Be(companyId);

            result.GetUncommittedEvents().Should().HaveCount(1);
            
            var firstEvent = result.GetUncommittedEvents()
                .First().As<BusinessAccountCreated>();
            firstEvent.AccountId.Should().Be(id);
            firstEvent.AccountName.Should().Be(name);
            firstEvent.AccountHolderId.Should().Be(companyId);
        }

        [Fact]
        public void Created_WithPersonId_CreatesModelAndRaisesEvents()
        {
            var id = AccountId.NewId();
            var name = new Name("account1");
            var personId = PersonId.NewId();

            var result = new Account(id, name, personId);

            result.Id.Should().Be(id);
            result.Name.Should().Be(name);
            result.AccountHolderId.Should().Be(personId);

            result.GetUncommittedEvents().Should().HaveCount(1);

            var firstEvent = result.GetUncommittedEvents()
                .First().As<PersonalAccountCreated>();
            firstEvent.AccountId.Should().Be(id);
            firstEvent.AccountName.Should().Be(name);
            firstEvent.AccountHolderId.Should().Be(personId);
        }

        [Fact]
        public void MakeDeposit_UpdatesBalanceAndAddsTransactionHistoryAndRaisesEvents()
        {
            var account = new AccountBuilder()
                .Build();

            account.MakeDeposit(10);

            account.TransactionHistory.Should().HaveCount(1);
            var firstTransaction = account.TransactionHistory.First();
            firstTransaction.Amount.Should().Be(10);

            account.Balance.Should().Be(10);

            var firstEvent = account.GetUncommittedEvents()
                .First().As<MoneyDeposited>();
            firstEvent.Amount.Should().Be(10);

            var firstDomainEvent = account.GetDomainEvents()
                .First().As<AccountBalanceChanged>();
            firstDomainEvent.AccountId.Should().Be(account.Id);
            firstDomainEvent.PreviousBalance.Should().Be(0);
            firstDomainEvent.NewBalance.Should().Be(10);
        }

        [Fact]
        public void MakeWithdrawal_UpdatesBalanceAndAddsTransactionHistoryAndRaisesEvents()
        {
            var account = new AccountBuilder()
                .WithDeposit(10)
                .Build();

            account.MakeWithdrawal(10);

            account.TransactionHistory.Should().HaveCount(2);
            var mostRecentTransaction = account.TransactionHistory.Last();
            mostRecentTransaction.Amount.Should().Be(-10);

            account.Balance.Should().Be(0);

            var firstEvent = account.GetUncommittedEvents()
                .First().As<MoneyWithdrawn>();
            firstEvent.Amount.Should().Be(10);

            var firstDomainEvent = account.GetDomainEvents()
                .First().As<AccountBalanceChanged>();
            firstDomainEvent.AccountId.Should().Be(account.Id);
            firstDomainEvent.PreviousBalance.Should().Be(10);
            firstDomainEvent.NewBalance.Should().Be(0);
        }

        [Fact]
        public void AddTransactionNote_AddsNoteAndRaisesEvents()
        {
            var text = "hello";

            var account = new AccountBuilder()
                .WithDeposit(10)
                .Build();

            var transaction = account.TransactionHistory.First();

            account.AddTransactionNote(transaction.Id, text);

            transaction.Notes.Should().HaveCount(1);
            var firstNote = transaction.Notes.First();
            firstNote.Text.Should().Be(text);

            var firstEvent = account.GetUncommittedEvents()
                .First().As<TransactionNoteCreated>();
            firstEvent.TransactionId.Should().Be(transaction.Id);
            firstEvent.Text.Should().Be(text);
        }

        [Fact]
        public void UpdateTransactionNote_UpdatesNoteAndRaisesEvents()
        {
            var account = new AccountBuilder()
                .WithDeposit(10)
                .Build();

            var transaction = account.TransactionHistory.First();
            account.AddTransactionNote(transaction.Id, "note");
            account.ClearUncommittedEvents();

            var note = transaction.Notes.First();
            note.EditText("new note");

            var firstEvent = account.GetUncommittedEvents()
                .First().As<TransactionNoteEdited>();
            firstEvent.NoteId.Should().Be(note.Id);
            firstEvent.Text.Should().Be("new note");
        }

        [Fact]
        public void DeleteTransactionNote_RemovesNoteAndRaisesEvents()
        {
            var account = new AccountBuilder()
                .WithDeposit(10)
                .Build();

            var transaction = account.TransactionHistory.First();
            account.AddTransactionNote(transaction.Id, "note");
            account.ClearUncommittedEvents();

            var note = transaction.Notes.First();
            note.Delete();

            transaction.Notes.Should().HaveCount(0);

            var firstEvent = account.GetUncommittedEvents()
                .First().As<TransactionNoteDeleted>();
            firstEvent.NoteId.Should().Be(note.Id);
        }
    }
}
