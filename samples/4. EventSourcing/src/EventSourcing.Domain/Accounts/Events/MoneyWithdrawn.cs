using Brickweave.EventStore;
using EventSourcing.Domain.Accounts.Models;

namespace EventSourcing.Domain.Accounts.Events
{
    public class MoneyWithdrawn : IEvent
    {
        public MoneyWithdrawn(TransactionId transactionId, decimal amount, DateTime timestamp)
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
