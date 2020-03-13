using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Brickweave.Messaging.SqlServer.Entities
{
    [Table(TABLE_NAME)]
    public class MessageData
    {
        public const string TABLE_NAME = "MessageOutbox";

        public Guid Id { get; set; }
        public string Json { get; set; }
        public DateTime Created { get; set; }
        public int CommitSequence { get; set; }
        public bool IsSending { get; set; }
    }
}
