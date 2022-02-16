namespace EventSourcing.Domain.Accounts.Models
{
    public class Transaction
    {
        public Transaction(TransactionId id, decimal amount, DateTime dateTime)
        {
            Id = id;
            Amount = amount;
            DateTime = dateTime;
        }

        public TransactionId Id { get; }
        public decimal Amount { get; }
        public DateTime DateTime { get; }
    }
}
