using Brickweave.Domain;
using System;

namespace Brickweave.Messaging.Models
{
    public class DomainMessageInfo
    {
        public DomainMessageInfo(DomainMessageId id, IDomainEvent domainEvent, DateTime created, int commitSequence)
        {
            Id = id;
            DomainEvent = domainEvent;
            Created = created;
            CommitSequence = commitSequence;
        }

        public DomainMessageId Id { get; }
        public IDomainEvent DomainEvent { get; }
        public DateTime Created { get; }
        public int CommitSequence { get; }
    }
}
