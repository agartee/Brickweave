using Brickweave.EventStore;
using EventSourcingDemo.Domain.Accounts.Models;

namespace EventSourcingDemo.Domain.Accounts.Events
{
    public class MoneyDeposited : IEvent
    {
        public MoneyDeposited(TransactionId transactionId, decimal amount, DateTime timestamp)
        {
            TransactionId = transactionId ?? throw new ArgumentNullException(nameof(transactionId));
            Amount = amount;
            Timestamp = timestamp;
        }

        public TransactionId TransactionId { get; }
        public decimal Amount { get; }
        public DateTime Timestamp { get; }
    }
}
