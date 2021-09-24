using System;
using Brickweave.Domain;
using LiteGuard;

namespace Brickweave.Messaging.Models
{
    public class DomainMessageInfo
    {
        public DomainMessageInfo(DomainMessageId id, IDomainEvent domainEvent, DateTime created, 
            int commitSequence, int sendAttemptCount, DateTime? lastSendAttempt = null)
        {
            Guard.AgainstNullArgument(nameof(id), id);
            Guard.AgainstNullArgument(nameof(domainEvent), domainEvent);
            
            Id = id;
            DomainEvent = domainEvent;
            Created = created;
            CommitSequence = commitSequence;
            SendAttemptCount = sendAttemptCount;
            LastSendAttempt = lastSendAttempt;
        }

        public DomainMessageId Id { get; }
        public IDomainEvent DomainEvent { get; }
        public DateTime Created { get; }
        public int CommitSequence { get; }
        public int SendAttemptCount { get; private set; }
        public DateTime? LastSendAttempt { get; private set; }
    }
}
